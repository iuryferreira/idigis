using System;

namespace Idigis.Web.Models
{
    public class Member
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? BaptismDate { get; set; }
        public string PhoneNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
