namespace Idigis.Shared.Dtos.Types
{
    public class ContactType
    {
        public ContactType (string phoneNumber, string houseNumber, string street, string district, string city)
        {
            PhoneNumber = phoneNumber;
            HouseNumber = houseNumber;
            Street = street;
            District = district;
            City = city;
        }

        public string PhoneNumber { get; }
        public string HouseNumber { get; }
        public string Street { get; }
        public string District { get; }
        public string City { get; }
    }
}
