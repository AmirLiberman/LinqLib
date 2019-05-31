using System.Collections.Generic;

namespace LinqLibTests
{
  public class TestPet
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public double Age { get; set; }

    public TestPet()
    {
      Name = "";
      Type = "";
    }

    public TestPet(int id)
    {
      switch (id)
      {
        case 1:
          Name = "Daisy";
          Type = "Dog";
          break;
        case 2:
          Name = "Tux";
          Type = "Cat";
          break;
        default:
          Name = "";
          Type = "";
          break;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
        return false;
      return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
      return this.Name.GetHashCode() ^ this.Type.GetHashCode();
    }
  }

  public class PetComparer : IEqualityComparer<TestPet>
  {
    #region IEqualityComparer<Pet> Members

    public bool Equals(TestPet x, TestPet y)
    {
      if (x == null && y == null)
        return true;
      else if (x == null || y == null)
        return false;
      else
        return GetHashCode(x) == GetHashCode(y);
    }

    public int GetHashCode(TestPet obj)
    {
      return obj.Name.GetHashCode() ^ obj.Type.GetHashCode();
    }

    #endregion
  }
}
