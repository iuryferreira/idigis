using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idigis.Core.Domain.Entities;
using Idigis.Core.Persistence.Contracts;
using Idigis.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Notie.Contracts;

namespace Idigis.Core.Persistence.Repositories
{
    internal class MemberRepository : IMemberRepository
    {
        private readonly Context _context;

        public MemberRepository (AbstractNotificator notificator, Context context)
        {
            Notificator = notificator;
            _context = context;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<bool> Add (string churchId, Member entity)
        {
            MemberModel model = entity;
            try
            {
                var churchExists =
                    await _context.ChurchContext.FirstOrDefaultAsync(church => church.Id == churchId) is not null;
                if (!churchExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                model.ChurchId = churchId;
                var entry = await _context.MemberContext.AddAsync(model);
                await _context.SaveChangesAsync();
                return entry.State is EntityState.Unchanged;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na inserção."));
                return false;
            }
        }

        public async Task<Member> GetById (string churchId, string id)
        {
            try
            {
                var members = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Members)
                    .FirstOrDefaultAsync();
                if (members is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return null;
                }

                var model = members.Find(member => member.Id == id);
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

        public async Task<List<Member>> All (string churchId)
        {
            try
            {
                var members = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Members.Select(o => (Member)o).ToList())
                    .FirstOrDefaultAsync();
                if (members is not null)
                {
                    return members;
                }

                Notificator.SetNotificationType(new("NotFound"));
                Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                return null;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na listagem."));
                return null;
            }
        }

        public async Task<bool> Update (string churchId, Member entity)
        {
            try
            {
                var members = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Members)
                    .FirstOrDefaultAsync();
                if (members is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                var model = members.Find(member => member.Id == entity.Id);
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                model.FullName = entity.FullName;
                model.BirthDate = entity.BirthDate;
                model.BaptismDate = entity.BaptismDate;
                model.PhoneNumber = entity.Contact?.PhoneNumber;
                model.HouseNumber = entity.Contact?.HouseNumber;
                model.Street = entity.Contact?.Street;
                model.District = entity.Contact?.District;
                model.City = entity.Contact?.City;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na atualização."));
                return false;
            }
        }

        public async Task<bool> Remove (string churchId, string id)
        {
            try
            {
                var members = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Members)
                    .FirstOrDefaultAsync();
                if (members is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                var model = members.Find(member => member.Id == id);
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                _context.MemberContext.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na remoção."));
                return false;
            }
        }
    }
}
