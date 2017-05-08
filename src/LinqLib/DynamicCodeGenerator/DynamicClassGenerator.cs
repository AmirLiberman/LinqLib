using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace LinqLib.DynamicCodeGenerator
{
  internal class DynamicClassGenerator
  {
    private Assembly assembly;
    private readonly string dynamicClassName;
    private readonly string dynamicClassNamespace;

    public object GetInstance(params object[] args)
    {
      return assembly.CreateInstance(dynamicClassNamespace + "." + dynamicClassName, false, BindingFlags.Public | BindingFlags.Instance, null, args, null, null);
    }

    public DynamicClassGenerator(string className, Type baseType, IEnumerable<string> properties, Type propertyType, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      dynamicClassName = className;
      dynamicClassNamespace = baseType.Namespace;
      CodeNamespace dynamicGenNamespace = new CodeNamespace(dynamicClassNamespace);

      dynamicGenNamespace.Imports.Add(new CodeNamespaceImport("LinqLib.DynamicCodeGenerator"));
      dynamicGenNamespace.Imports.Add(new CodeNamespaceImport("System"));
      if (baseType.Namespace != propertyType.Namespace)
        dynamicGenNamespace.Imports.Add(new CodeNamespaceImport(propertyType.Namespace));

      CodeTypeDeclaration dynamicClass = CreateClass(dynamicClassName, baseType);
      dynamicGenNamespace.Types.Add(dynamicClass);

      string[] propArr = properties.ToArray();
      int propertiesCount = propArr.Count();

      dynamicClass.Members.Add(CreateArrayField("properties", typeof(PropertyInfo[]), true, 0));
      dynamicClass.Members.Add(CreateField("propertiesMap", typeof(Dictionary<string, int>), true));
      dynamicClass.Members.Add(CreateStaticConstructor(propertiesCount, dynamicClassName));

      dynamicClass.Members.Add(CreateGetPropertyByNameMethod());
      dynamicClass.Members.Add(CreateGetPropertyByIndexMethod());
      dynamicClass.Members.Add(CreateGetPropertyTypeByIndexMethod());
      dynamicClass.Members.Add(CreateGetPropertyTypeByNameMethod());

      foreach (string property in propArr)
      {
        dynamicClass.Members.Add(CreateField(property, propertyType, false));
        dynamicClass.Members.Add(CreateProperty(property, propertyType));
      }

      dynamicClass.Members.Add(CreateStringIndexerProperty());
      dynamicClass.Members.Add(CreateNumericIndexerProperty());
      dynamicClass.Members.Add(CreatePropertiesCountProperty());
      dynamicClass.Members.Add(CreatePropertiesNamesNamesProperty());

      GenerateAssembly(dynamicGenNamespace, new[] { baseType, propertyType }, classGenerationEventHandler);
    }

    private void GenerateAssembly(CodeNamespace codeNamespace, IEnumerable<Type> relatedTypes, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      CodeCompileUnit assemblyGen = new CodeCompileUnit();
      assemblyGen.Namespaces.Add(codeNamespace);
      string[] locations = relatedTypes.Select(T => T.Assembly.Location).Distinct().ToArray();

      CompilerParameters compParam = new CompilerParameters(locations);
      compParam.ReferencedAssemblies.Add("System.dll");
      compParam.ReferencedAssemblies.Add(GetType().Assembly.Location);

      compParam.GenerateInMemory = true;
      compParam.GenerateExecutable = false;

      compParam.CompilerOptions = "/Debug-";
      compParam.IncludeDebugInformation = false;

#if KEEP_TEMP && DEBUG
      compParam.TempFiles.KeepFiles = true;
#endif

      using (CSharpCodeProvider cscp = new CSharpCodeProvider())
      {
        CompilerResults compRes = cscp.CompileAssemblyFromDom(compParam, assemblyGen);
        if (compRes.Errors.HasErrors)
          assembly = null;
        else
          assembly = compRes.CompiledAssembly;

#if KEEP_TEMP && DEBUG
        foreach (string tempFile in compRes.TempFiles.Cast<string>().Where(FILE => !FILE.EndsWith(".cs")))
          System.IO.File.Delete(tempFile);
#endif

        if (classGenerationEventHandler == null)
          return;

        ClassGenerationEventArgs args = new ClassGenerationEventArgs(compRes.TempFiles.BasePath,
                                                                     compRes.TempFiles.OfType<string>().FirstOrDefault(F => F.EndsWith(".cs")),
                                                                     compRes.Output,
                                                                     compRes.Errors.HasErrors);
        classGenerationEventHandler(this, args);
      }
    }

    private static CodeTypeDeclaration CreateClass(string className, Type baseClass)
    {
      CodeTypeDeclaration dynamicClass = new CodeTypeDeclaration();
      CodeTypeReference dynamicClassBase = new CodeTypeReference(baseClass);

      dynamicClass.IsClass = true;
      dynamicClass.Name = className;
      dynamicClass.BaseTypes.Add(dynamicClassBase);
      dynamicClass.BaseTypes.Add(new CodeTypeReference("IDynamicPivotObject"));
      return dynamicClass;
    }

    private static CodeTypeConstructor CreateStaticConstructor(int propertyCount, string className)
    {
      CodeTypeConstructor constructor = new CodeTypeConstructor();

      // Line 1: _propertiesMap = new Dictionary<string, int>();            
      CodeAssignStatement line1 = new CodeAssignStatement
        {
          Left = new CodeVariableReferenceExpression("_propertiesMap"),
          Right = new CodeObjectCreateExpression(new CodeTypeReference(typeof(Dictionary<string, int>)))
        };
      constructor.Statements.Add(line1);

      // Line 2: _properties = this.GetType().GetProperties();
      CodeAssignStatement line2 = new CodeAssignStatement
        {
          Left = new CodeVariableReferenceExpression("_properties"),
          Right = new CodeMethodInvokeExpression(
            new CodeTypeOfExpression(className), "GetProperties")
        };
      constructor.Statements.Add(line2);

      // Statement 1: int p = 0
      CodeVariableDeclarationStatement stmt1 = new CodeVariableDeclarationStatement(typeof(int), "p")
        {
          InitExpression = new CodePrimitiveExpression(0)
        };

      // Statement 2: p + 1
      CodeBinaryOperatorExpression stmt2 = new CodeBinaryOperatorExpression
        {
          Left = new CodeVariableReferenceExpression("p"),
          Operator = CodeBinaryOperatorType.Add,
          Right = new CodePrimitiveExpression(1)
        };

      // Statement 3: p = p + 1
      CodeAssignStatement stmt3 = new CodeAssignStatement
        {
          Left = new CodeVariableReferenceExpression("p"),
          Right = stmt2
        };

      // Statement 4: p < propertyCount
      CodeBinaryOperatorExpression stmt4 = new CodeBinaryOperatorExpression
        {
          Left = new CodeVariableReferenceExpression("p"),
          Operator = CodeBinaryOperatorType.LessThan,
          Right = new CodePrimitiveExpression(propertyCount)
        };

      // Statement 5: _properties[p]
      CodeArrayIndexerExpression stmt5 = new CodeArrayIndexerExpression(
                                           new CodeVariableReferenceExpression("_properties"),
                                           new CodeVariableReferenceExpression("p"));

      // Line 3: for (p = 0; p < propertyCount; p++)
      CodeIterationStatement line3 = new CodeIterationStatement
        {
          InitStatement = stmt1,
          TestExpression = stmt4,
          IncrementStatement = stmt3
        };

      // Line 3.1 : _propertiesMap.add(_properties[p].Name, p)
      CodeMethodInvokeExpression line3X1 = new CodeMethodInvokeExpression(
                                          new CodeVariableReferenceExpression("_propertiesMap"),
                                          "Add",
                                          new CodePropertyReferenceExpression(stmt5, "Name"),
                                          new CodeVariableReferenceExpression("p"));
      line3.Statements.Add(line3X1);
      constructor.Statements.Add(line3);
      return constructor;
    }

    private static CodeMemberField CreateArrayField(string name, Type type, bool createStatic, int size)
    {
      CodeMemberField fld = new CodeMemberField
        {
          Name = "_" + name,
          Type = new CodeTypeReference(type)
        };

      if (size > 0)
        fld.InitExpression = new CodeArrayCreateExpression(new CodeTypeReference(type), size);

      fld.Attributes = MemberAttributes.Private;

      if (createStatic)
        fld.Attributes |= MemberAttributes.Static;
      return fld;
    }

    private static CodeMemberField CreateField(string name, Type type, bool createStatic)
    {
      CodeMemberField fld = new CodeMemberField
        {
          Name = "_" + name,
          Type = new CodeTypeReference(type),
          Attributes = MemberAttributes.Private
        };
      if (createStatic)
        fld.Attributes |= MemberAttributes.Static;
      return fld;
    }

    private static CodeMemberProperty CreateProperty(string name, Type type)
    {
      CodeMemberProperty prop = new CodeMemberProperty
        {
          Name = name,
          Type = new CodeTypeReference(type),
          Attributes = MemberAttributes.Public,
          HasGet = true,
          HasSet = true
        };

      // Get Statement of the property
      CodeFieldReferenceExpression propReference = new CodeFieldReferenceExpression
        {
          FieldName = "_" + name
        };
      prop.GetStatements.Add(new CodeMethodReturnStatement(propReference));

      // Set Statement of the property
      CodeAssignStatement propAssignment = new CodeAssignStatement
        {
          Left = propReference,
          Right = new CodeArgumentReferenceExpression("value")
        };
      prop.SetStatements.Add(propAssignment);

      return prop;
    }

    private static CodeMemberProperty CreateStringIndexerProperty()
    {
      CodeMemberProperty prop = new CodeMemberProperty
        {
          Name = "Item"
        };

      prop.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "index"));
      prop.Type = new CodeTypeReference(typeof(object));
      prop.Attributes = MemberAttributes.Public;
      prop.HasGet = true;
      prop.HasSet = true;

      // Get Statement of the property      
      CodeMethodReturnStatement getStmt = new CodeMethodReturnStatement(
                                         new CodeMethodInvokeExpression(
                                           new CodeArrayIndexerExpression(
                                             new CodeVariableReferenceExpression("_properties"),
                                             new CodeArrayIndexerExpression(
                                               new CodeVariableReferenceExpression("_propertiesMap"),
                                               new CodeArgumentReferenceExpression("index"))),
                                           "GetValue",
                                           new CodeThisReferenceExpression(),
                                           new CodePrimitiveExpression(null)));
      prop.GetStatements.Add(getStmt);

      CodeMethodInvokeExpression setStmt = new CodeMethodInvokeExpression(
                                       new CodeArrayIndexerExpression(
                                         new CodeVariableReferenceExpression("_properties"),
                                         new CodeArrayIndexerExpression(
                                           new CodeVariableReferenceExpression("_propertiesMap"),
                                             new CodeArgumentReferenceExpression("index"))),
                                         "SetValue",
                                         new CodeThisReferenceExpression(),
                                         new CodeArgumentReferenceExpression("value"),
                                         new CodePrimitiveExpression(null));
      prop.SetStatements.Add(setStmt);
      return prop;
    }

    private static CodeMemberProperty CreateNumericIndexerProperty()
    {
      CodeMemberProperty prop = new CodeMemberProperty
        {
          Name = "Item"
        };

      prop.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "index"));
      prop.Type = new CodeTypeReference(typeof(object));
      prop.Attributes = MemberAttributes.Public;
      prop.HasGet = true;
      prop.HasSet = true;

      // Get Statement of the property      
      CodeMethodReturnStatement getStmt = new CodeMethodReturnStatement(
                                               new CodeMethodInvokeExpression(
                                               new CodeArrayIndexerExpression(
                                                   new CodeVariableReferenceExpression("_properties"),
                                                   new CodeArgumentReferenceExpression("index")),
                                                 "GetValue",
                                                 new CodeThisReferenceExpression(),
                                                 new CodePrimitiveExpression(null)));
      prop.GetStatements.Add(getStmt);

      CodeMethodInvokeExpression setStmt = new CodeMethodInvokeExpression(
                                            new CodeArrayIndexerExpression(
                                              new CodeVariableReferenceExpression("_properties"),
                                              new CodeArgumentReferenceExpression("index")),
                                            "SetValue",
                                            new CodeThisReferenceExpression(),
                                            new CodeArgumentReferenceExpression("value"),
                                            new CodePrimitiveExpression(null));
      prop.SetStatements.Add(setStmt);
      return prop;
    }

    private static CodeMemberMethod CreateGetPropertyByNameMethod()
    {
      // Object GetPropertyName(int propertyIndex)
      CodeMemberMethod method = new CodeMemberMethod
        {
          Name = "GetPropertyName",
          ReturnType = new CodeTypeReference(typeof(string)),
          Attributes = MemberAttributes.Public
        };

      method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "propertyIndex"));

      // Return _properties[propertyIndex].Name;
      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodePropertyReferenceExpression(
                                           new CodeArrayIndexerExpression(
                                             new CodeVariableReferenceExpression("_properties"),
                                             new CodeArgumentReferenceExpression("propertyIndex")),
                                           "Name"));
      method.Statements.Add(stmt);

      return method;
    }

    private static CodeMemberMethod CreateGetPropertyByIndexMethod()
    {
      // Object GetPropertyName(string propertyName)
      CodeMemberMethod method = new CodeMemberMethod
        {
          Name = "GetPropertyIndex",
          ReturnType = new CodeTypeReference(typeof(int)),
          Attributes = MemberAttributes.Public
        };

      method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "propertyName"));

      // Return _propertiesMap[propertyName];
      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodeArrayIndexerExpression(
                                           new CodeVariableReferenceExpression("_propertiesMap"),
                                           new CodeArgumentReferenceExpression("propertyName")));
      method.Statements.Add(stmt);

      return method;
    }

    private static CodeMemberMethod CreateGetPropertyTypeByIndexMethod()
    {
      // Type GetPropertyType(int propertyIndex)
      CodeMemberMethod method = new CodeMemberMethod
        {
          Name = "GetPropertyType",
          ReturnType = new CodeTypeReference(typeof(Type)),
          Attributes = MemberAttributes.Public
        };

      method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "propertyIndex"));

      // Return _properties[propertyIndex].GetType;
      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodePropertyReferenceExpression(
                                           new CodeArrayIndexerExpression(
                                             new CodeVariableReferenceExpression("_properties"),
                                             new CodeArgumentReferenceExpression("propertyIndex")),
                                           "PropertyType"));
      method.Statements.Add(stmt);

      return method;
    }

    private static CodeMemberMethod CreateGetPropertyTypeByNameMethod()
    {
      // Type GetPropertyType(string propertyName)
      CodeMemberMethod method = new CodeMemberMethod
        {
          Name = "GetPropertyType",
          ReturnType = new CodeTypeReference(typeof(Type)),
          Attributes = MemberAttributes.Public
        };

      method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "propertyName"));

      // Return _properties[_propertiesMap[propertyName]].GetType;
      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodePropertyReferenceExpression(
                                           new CodeArrayIndexerExpression(
                                             new CodeVariableReferenceExpression("_properties"),
                                             new CodeArrayIndexerExpression(
                                               new CodeVariableReferenceExpression("_propertiesMap"),
                                               new CodeArgumentReferenceExpression("propertyName"))),
                                           "PropertyType"));
      method.Statements.Add(stmt);

      return method;
    }

    private static CodeMemberProperty CreatePropertiesCountProperty()
    {
      CodeMemberProperty prop = new CodeMemberProperty
        {
          Name = "PropertiesCount",
          Type = new CodeTypeReference(typeof(int)),
          Attributes = MemberAttributes.Public,
          HasGet = true,
          HasSet = false
        };

      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodePropertyReferenceExpression(
                                           new CodeVariableReferenceExpression("_propertiesMap"), "Count"));
      prop.GetStatements.Add(stmt);

      return prop;
    }

    private static CodeMemberProperty CreatePropertiesNamesNamesProperty()
    {
      CodeMemberProperty prop = new CodeMemberProperty
        {
          Name = "PropertiesNames",
          Type = new CodeTypeReference(typeof(IEnumerable<string>)),
          Attributes = MemberAttributes.Public,
          HasGet = true,
          HasSet = false
        };

      CodeMethodReturnStatement stmt = new CodeMethodReturnStatement(
                                         new CodePropertyReferenceExpression(
                                           new CodeVariableReferenceExpression("_propertiesMap"), "Keys"));
      prop.GetStatements.Add(stmt);

      return prop;
    }
  }
}
