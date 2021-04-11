using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;
using Idigis.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Repositories
{
    internal class ChurchRepository : IChurchRepository
    {
        private readonly Context _context;

        public ChurchRepository (AbstractNotificator notificator, Context context)
        {
            Notificator = notificator;
            _context = context;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<bool> Add (Church entity)
        {
            ChurchModel model = entity;
            try
            {
                var exists =
                    await _context.ChurchContext.FirstOrDefaultAsync(church =>
                        church.Id == model.Id || church.Email == model.Email) is not null;
                if (exists)
                {
                    Notificator.SetNotificationType(new("Validation"));
                    Notificator.AddNotification(new("Repository", "Este registro já existe, faça o login."));
                    return false;
                }

                var entry = await _context.ChurchContext.AddAsync(model);
                await _context.SaveChangesAsync();
                return entry.State is EntityState.Unchanged;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na inserção."));
                return false;
            }
        }

        public async Task<Church> GetById (string id)
        {
            try
            {
                var model = await _context.ChurchContext.FirstOrDefaultAsync(church => church.Id == id);
                if (model is not null)
                {
                    return model;
                }

                Notificator.SetNotificationType(new("NotFound"));
                Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                return null;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na busca."));
                return null;
            }
        }

        public async Task<Church> GetByEmail (string email)
        {
            try
            {
                var model = await _context.ChurchContext.FirstOrDefaultAsync(church => church.Email == email);
                if (model is not null)
                {
                    return model;
                }

                Notificator.SetNotificationType(new("NotFound"));
                Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                return null;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na busca."));
                return null;
            }
        }

        public async Task<bool> Update (Church entity)
        {
            try
            {
                var model = await _context.ChurchContext.FirstOrDefaultAsync(church => church.Id == entity.Id);
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                model.Email = entity.Credentials.Email;
                model.Password = entity.Credentials.Password;
                model.Name = entity.Name;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na atualização."));
                return false;
            }
        }

        public async Task<bool> Remove (string id)
        {
            try
            {
                var model = await _context.ChurchContext.FirstOrDefaultAsync(church => church.Id == id);
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                var entry = _context.ChurchContext.Remove(model);
                await _context.SaveChangesAsync();
                return entry.State is EntityState.Detached;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na remoção."));
                return false;
            }
        }
    }
}
