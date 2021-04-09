using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Domain.ValueObjects;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Idigis.Core.Dtos.Types;
using Idigis.Core.Persistence.Contracts;
using Notie.Contracts;

namespace Idigis.Core.Application.UseCases
{
    internal class MemberUseCase : IMemberUseCase
    {
        private readonly IMemberRepository _repository;

        public MemberUseCase (AbstractNotificator notificator, IMemberRepository repository)
        {
            Notificator = notificator;
            _repository = repository;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateMemberResponse> Add (CreateMemberRequest data)
        {
            var contact = data.Contact is not null
                ? new Contact(data.Contact.PhoneNumber,
                    data.Contact.HouseNumber,
                    data.Contact.Street,
                    data.Contact.District,
                    data.Contact.City)
                : null;
            var entity = new Member(data.FullName, data.BirthDate, data.BaptismDate, contact);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Add(data.ChurchId, entity))
            {
                return null;
            }

            return new()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                BaptismDate = entity.BaptismDate,
                BirthDate = entity.BirthDate,
                Contact = entity.Contact is not null
                    ? new ContactType(entity.Contact.PhoneNumber,
                        entity.Contact.HouseNumber,
                        entity.Contact.Street,
                        entity.Contact.District,
                        entity.Contact.City)
                    : null
            };
        }

        public async Task<GetMemberResponse> Get (GetMemberRequest data)
        {
            var member = await _repository.GetById(data.ChurchId, data.Id);
            return member is null
                ? null
                : new()
                {
                    Id = member.Id,
                    FullName = member.FullName,
                    BaptismDate = member.BaptismDate,
                    BirthDate = member.BirthDate,
                    Contact = member.Contact is not null
                        ? new ContactType(member.Contact.PhoneNumber,
                            member.Contact.HouseNumber,
                            member.Contact.Street,
                            member.Contact.District,
                            member.Contact.City)
                        : null
                };
        }

        public async Task<List<GetMemberResponse>> List (ListMemberRequest data)
        {
            var members = await _repository.All(data.ChurchId);
            return members is null
                ? new()
                : members.Select(member => new GetMemberResponse
                {
                    Id = member.Id,
                    FullName = member.FullName,
                    BaptismDate = member.BaptismDate,
                    BirthDate = member.BirthDate,
                    Contact = member.Contact is not null
                        ? new ContactType(member.Contact.PhoneNumber, member.Contact.HouseNumber, member.Contact.Street,
                            member.Contact.District, member.Contact.City)
                        : null
                }).ToList();
        }

        public async Task<EditMemberResponse> Edit (EditMemberRequest data)
        {
            var contact = data.Contact is not null
                ? new Contact(data.Contact.PhoneNumber,
                    data.Contact.HouseNumber,
                    data.Contact.Street,
                    data.Contact.District,
                    data.Contact.City)
                : null;
            var entity = new Member(data.Id, data.FullName, data.BirthDate, data.BaptismDate, contact);
            if (entity.Invalid)
            {
                Notificator.AddNotificationsByFluent(entity.ValidationResult);
                return null;
            }

            if (!await _repository.Update(data.ChurchId, entity))
            {
                return null;
            }

            return new()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                BaptismDate = entity.BaptismDate,
                BirthDate = entity.BirthDate,
                Contact = entity.Contact is not null
                    ? new ContactType(entity.Contact.PhoneNumber,
                        entity.Contact.HouseNumber,
                        entity.Contact.Street,
                        entity.Contact.District,
                        entity.Contact.City)
                    : null
            };
        }

        public async Task<DeleteMemberResponse> Delete (DeleteMemberRequest data)
        {
            if (!await _repository.Remove(data.ChurchId, data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
