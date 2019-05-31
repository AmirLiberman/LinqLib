using System.Collections.Generic;

namespace Samples.SampleData
{
  public static class Generator
  {
    public static List<Contact> CreateContacts()
    {
      return new List<Contact>
      {
        new Contact { Id = 1, FirstName = "John"  , LastName = "Doe", Phones = new List<Phone> { new Phone("Home",   "305", "555-1111"),
                                                                                                 new Phone("Office", "305", "555-2222"),
                                                                                                 new Phone("Cell",   "305", "555-3333")}
                                                                , Addresses = new List<Address>{ new Address{ AddressType ="Home",   City="Miami", State="FL", Street="123 Main Street"},
                                                                                                 new Address{ AddressType ="Office", City="Miami", State="FL", Street="123 Main Street"}}},
        new Contact { Id = 2, FirstName = "Jane"  , LastName = "Doe", Phones = new List<Phone> { new Phone("Home",   "305", "555-1111"),
                                                                                                 new Phone("Office", "305", "555-4444"),
                                                                                                 null,
                                                                                                 new Phone("Cell",   "305", "555-5555")}
                                                                , Addresses = new List<Address>{ new Address{ AddressType ="Home",   City="Davie",   State="FL", Street="123 Main Street"},
                                                                                                 new Address{ AddressType ="Office", City="Pompano", State="FL", Street="123 Main Street"}}},
        new Contact { Id = 3, FirstName = "Jerome", LastName = "Doe", Phones = new List<Phone> { new Phone("Home",   "305", "555-6666"),
                                                                                                 new Phone("Office", "305", "555-2222"),
                                                                                                 new Phone("Cell",   "305", "555-7777")}},
        new Contact { Id = 4, FirstName = "Joel", LastName = "Smith", Phones = new List<Phone> { new Phone("Fax",    "305", "555-6666"),
                                                                                                 new Phone("Office", "305", "555-2222"),
                                                                                                 new Phone("Cell2",  "305", "555-7778"),
                                                                                                 new Phone("Cell",   "305", "555-7777")}
      }};
    }
  }
}