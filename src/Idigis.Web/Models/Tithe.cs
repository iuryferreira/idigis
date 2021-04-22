using System;

namespace Idigis.Web.Models
{
    public class Tithe
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string MemberName { get; set; }
        public string MemberId { get; set; }
    }
}
