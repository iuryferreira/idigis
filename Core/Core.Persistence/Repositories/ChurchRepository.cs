using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Shared.Notifications;
using Core.Shared.Types;

namespace Core.Persistence.Repositories
{
    public class ChurchRepository : IChurchRepository
    {
        private readonly IChurchContext _context;

        public ChurchRepository (IChurchContext context, INotificator notificator)
        {
            _context = context;
            Notificator = notificator;
        }

        public INotificator Notificator { get; }

        public async Task<bool> Add (Church entity)
        {
            ChurchModel model = entity;
            if (!await _context.Exists(model))
            {
                var result = await _context.Add(model);
                if (!result)
                {
                    Notificator.AddNotification("Repository", "Ocorreu um erro na inserção.");
                }

                return result;
            }
            Notificator.NotificationType = NotificationType.Validation;
            Notificator.AddNotification("Repository", "Este registro já existe, faça o login.");
            return false;
        }

        public async Task<Church> Get (Property property)
        {
            Church result = await _context.Get(property);
            if (result is not null)
            {
                return result;
            }
            Notificator.NotificationType = NotificationType.NotFound;
            Notificator.AddNotification("Repository", "Registro não encontrado. Verifique as informações inseridas.");
            return null;
        }
    }
}
