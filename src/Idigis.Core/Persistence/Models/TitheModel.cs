using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;

namespace Idigis.Core.Persistence.Models
{
    [Table("tithes")]
    internal class TitheModel : Model
    {
        [Required] public decimal Value { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string MemberId { get; set; }
        [Required] public MemberModel Member { get; set; }
        public string ChurchModelId { get; set; }
        public ChurchModel ChurchModel { get; set; }

        public static implicit operator TitheModel (Tithe entity)
        {
            if (entity is null) { return null; }

            return new() { Id = entity.Id, Date = entity.Date, Value = entity.Value };
        }

        public static implicit operator Tithe (TitheModel model)
        {
            if (model is null) { return null; }

            return new(model.Id, model.Value, model.Date, model.MemberId, model.Member.FullName);
        }
    }
}
