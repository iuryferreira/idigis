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
        private readonly IChurchRepository _churchRepository;
        private readonly IMemberRepository _repository;

        public MemberUseCase (AbstractNotificator notificator, IMemberRepository repository,
            IChurchRepository churchRepository)
        {
            Notificator = notificator;
            _repository = repository;
            _churchRepository = churchRepository;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<CreateMemberResponse> Add (CreateMemberRequest data)
        {
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

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

            if (!await _repository.Add(entity))
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

        public async Task<EditMemberResponse> Edit (EditMemberRequest data)
        {
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

            var member = await _repository.GetById(data.Id);
            if (member is null)
            {
                return null;
            }

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

            if (!await _repository.Update(entity))
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
            var church = await _churchRepository.GetById(data.ChurchId);
            if (church is null)
            {
                return null;
            }

            if (!await _repository.Remove(data.Id))
            {
                return null;
            }

            return new();
        }
    }
}
