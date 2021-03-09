using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Shared.Notifications;
using Core.Shared.Type;

namespace Core.Persistence.Repositories
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

            Notifications.Add(new("Repository", "Este registro já existe, faça o login."));
            return false;
        }

        public async Task<Church> Get (Property property)
        {
            Church result = await _context.Get(property);
            if (result is not null)
            {
                return result;
            }
            Notifications.Add(new("Repository", "Registro não encontrado. Verifique as informações inseridas."));
            return null;
        }
    }
}
