using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Persistence.Contracts;
using Persistence.Models;
using Shared.Notifications;

namespace Persistence.Repositories
{
    public class ChurchRepository : IChurchRepository
    {
        private readonly IChurchContext _context;
        public List<Notification> Notifications { get; }

        public ChurchRepository (IChurchContext context)
        {
            _context = context;
            Notifications = new();
        }

        public async Task<bool> Add (Church entity)
        {
            ChurchModel model = entity;
            if (!(await _context.Exists(model)))
            {
                bool result = await _context.Add(model);
                if (!result) Notifications.Add(new("Repository", "Ocorreu um erro na inserção."));
                return result;

            }
            Notifications.Add(new("Repository", "O usuário já existe, faça o login."));
            return false;
        }
    }
}
