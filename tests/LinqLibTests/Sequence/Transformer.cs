using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.DynamicCodeGenerator;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class Transformer
  {
    #region Pivot

    [TestMethod()]
    public void PivotTest1()
    {
      List<Contact> contacts = new List<Contact>
      {
        new Contact { Id = 1, FirstName = "John"  , LastName = "Doe", Phones= new List<Phone> { new Phone("Home",   "305", "555-1111"),
                                                                                                new Phone("Office", "305", "555-2222"),
                                                                                                new Phone("Cell",   "305", "555-3333")}},
        new Contact { Id = 2, FirstName = "Jane"  , LastName = "Doe", Phones= new List<Phone> { new Phone("Home",   "305", "555-1111"),
                                                                                                new Phone("Office", "305", "555-4444"),
                                                                                                new Phone("Cell",   "305", "555-5555")}},
        new Contact { Id = 3, FirstName = "Jerome", LastName = "Doe", Phones= new List<Phone> { new Phone("Home",   "305", "555-6666"),
                                                                                                new Phone("Office", "305", "555-2222"),
                                                                                                new Phone("Cell",   "305", "555-7777")}},
      };

      object[] pvt1 = contacts.Pivot(X => X.Phones, X => X.PhoneType, false).ToArray();
      object[] pvt2 = contacts.Pivot(X => X.Phones, X => X.PhoneType, false).ToArray();
      Assert.IsTrue(((IDynamicPivotObject)pvt1.First()).PropertiesCount == 3);
      Assert.IsTrue(((IDynamicPivotObject)pvt1.First())["Office"] == contacts[0].Phones[1]);

      LinqLib.Sequence.Transformer.SafeNamePrefix = "_S_";

      contacts.Add(new Contact
      {
        Id = 4,
        FirstName = "Joel",
        LastName = "Smith",
        Phones = new List<Phone> { new Phone("Fax",      "305", "555-6666"),
                                   new Phone("Office",   "305", "555-2222"),
                                   new Phone("Cell",     "305", "555-7777"),
                                   new Phone("2nd Cell", "305", "555-7779")}
      });
      object[] pvt3 = contacts.Pivot(X => X.Phones, X => X.PhoneType, true, FilesCreated).ToArray();

      Assert.IsTrue(((IDynamicPivotObject)pvt3.First()).PropertiesCount == 5);
      Assert.IsTrue(((IDynamicPivotObject)pvt3.Last())["Fax"] == contacts[3].Phones[0]);

      contacts[0].Phones.Clear();
      contacts[1].Phones.Clear();
      contacts[2].Phones.Clear();
      contacts[3].Phones.Clear();
      object[] pvt4 = contacts.Pivot(X => X.Phones, X => X.PhoneType, true, FilesCreated).ToArray();
      Assert.IsTrue(contacts.SequenceEqual(pvt4));

      try
      {
        contacts[0].Phones = null;
        contacts[1].Phones = null;
        contacts[2].Phones = null;
        contacts[3].Phones = null;
        object[] pvt5 = contacts.Pivot(X => X.Phones, X => X.PhoneType, true, FilesCreated).ToArray();
        Assert.Fail();
      }
      catch (ArgumentException) { }
      catch (Exception) { Assert.Fail(); }

    }

    [TestMethod()]
    public void PivotTest2()
    {
      Contact contact = new Contact
      {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Phones = new List<Phone> { new Phone("Home",   "305", "555-1111"),
          new Phone("Office", "305", "555-2222"),
          new Phone("Cell",   "305", "555-3333")}
      };

      object pvt = contact.Pivot(X => X.Phones, X => X.PhoneType, X => X.PhoneNumber);
      Assert.IsTrue(((IDynamicPivotObject)pvt).PropertiesCount == 3);
      Assert.IsTrue((string)((IDynamicPivotObject)pvt)["Office"] == contact.Phones[1].PhoneNumber);

      contact.Phones.Clear();
      object pvt2 = contact.Pivot(X => X.Phones, X => X.PhoneType, X => X.PhoneNumber);
      Assert.IsTrue(pvt2 == contact);

      try
      {
        contact.Phones = null;
        object pvt3 = contact.Pivot(X => X.Phones, X => X.PhoneType, X => X.PhoneNumber);
        Assert.Fail();
      }
      catch (ArgumentException) { }
      catch (Exception) { Assert.Fail(); }

    }

    #endregion

    public class Phone
    {
      public Phone(string phoneType, string areaCode, string phoneNumber)
      {

        this.PhoneType = phoneType;
        this.AreaCode = areaCode;
        this.PhoneNumber = phoneNumber;
      }

      public string PhoneType { get; set; }
      public string AreaCode { get; set; }
      public string PhoneNumber { get; set; }
    }

    public class Contact
    {
      public int Id;

      public string FirstName { get; set; }
      public string LastName { get; set; }
      public List<Phone> Phones { get; set; }
    }

    private void FilesCreated(object sender, ClassGenerationEventArgs e)
    {
      Assert.IsTrue(!e.HasError);
    }
  }
}
