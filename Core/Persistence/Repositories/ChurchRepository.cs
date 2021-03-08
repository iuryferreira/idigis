using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Persistence.Contracts;
using Persistence.Models;
using Shared.Notifications;

namespace Persistence.Repositories
{
    public class ChurchRepository : IChurchRepository
    {
        private readonly IChurchContext _context;

        public ChurchRepository (IChurchContext context)
        {
            _context = context;
            Notifications = new();
        }

        public List<Notification> Notifications { get; }

        public async Task<bool> Add (Church entity)
        {
            ChurchModel model = entity;
            if (!await _context.Exists(model))
            {
                var result = await _context.Add(model);
                if (!result)
                {
                    Notifications.Add(new("Repository", "Ocorreu um erro na inserção."));
                }

                return result;
            }

            Notifications.Add(new("Repository", "O usuário já existe, faça o login."));
            return false;
        }
    }
}
