using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;

namespace Idigis.Core.Persistence.Models
{
    [Table("members")]
    internal class MemberModel : Model
    {
        [Required] public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? BaptismDate { get; set; }
        public string PhoneNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }

        [Required] public string ChurchId { get; set; }
        [Required] public ChurchModel Church { get; set; }
        public List<TitheModel> Tithes { get; set; }

        public static implicit operator MemberModel (Member entity)
        {
            if (entity is null) { return null; }

            return new()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                BirthDate = entity.BirthDate,
                BaptismDate = entity.BaptismDate,
                City = entity.Contact?.City,
                District = entity.Contact?.District,
                PhoneNumber = entity.Contact?.PhoneNumber,
                Street = entity.Contact?.Street,
                HouseNumber = entity.Contact?.HouseNumber
            };
        }

        public static implicit operator Member (MemberModel model)
        {
            if (model is null) { return null; }

            return new(model.FullName, model.BirthDate, model.BirthDate,
                new(model.PhoneNumber, model.HouseNumber, model.Street, model.District, model.City));
        }
    }
}
