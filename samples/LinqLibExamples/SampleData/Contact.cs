using System.Collections.Generic;

namespace Samples.SampleData
{
  public class Contact
  {
    public int Id;

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhonesCount { get { return Phones.Count; } }

    public List<Phone> Phones { get; set; }
    public List<Address> Addresses { get; set; }
  }
}