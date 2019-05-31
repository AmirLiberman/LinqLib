
namespace Samples.SampleData
{
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
}