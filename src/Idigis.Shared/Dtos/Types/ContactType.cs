namespace Idigis.Shared.Dtos.Types
{
    public class ContactType
    {
        public ContactType ()
        {
            
        }

        public ContactType (string phoneNumber, string houseNumber, string street, string district, string city)
        {
            PhoneNumber = phoneNumber;
            HouseNumber = houseNumber;
            Street = street;
            District = district;
            City = city;
        }

        public string PhoneNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
