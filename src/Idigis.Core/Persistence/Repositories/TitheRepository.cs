using System;
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
    internal class TitheRepository : ITitheRepository
    {
        private readonly Context _context;

        public TitheRepository (AbstractNotificator notificator, Context context)
        {
            Notificator = notificator;
            _context = context;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<bool> Add (string churchId, string memberId, Tithe entity)
        {
            TitheModel model = entity;
            try
            {
                var churchAndMemberExists = await _context.ChurchContext
                    .Where(c => c.Id == churchId)
                    .Select(c => c.Members.FirstOrDefault(m => m.Id == memberId))
                    .FirstOrDefaultAsync() is not null;
                if (!churchAndMemberExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja ou membro não existe no sistema."));
                    return false;
                }

                model.MemberId = memberId;
                model.ChurchModelId = churchId;
                var entry = await _context.TitheContext.AddAsync(model);
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

        public async Task<Tithe> GetById (string churchId, string memberId, string id)
        {
            try
            {
                var churchAndMemberExists = await _context.ChurchContext
                    .Where(c => c.Id == churchId)
                    .Select(c => c.Members.FirstOrDefault(m => m.Id == memberId))
                    .FirstOrDefaultAsync() is not null;
                if (!churchAndMemberExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja ou membro não existe no sistema."));
                    return null;
                }

                var model = await _context.MemberContext.Where(m => m.Id == memberId)
                    .Select(m => m.Tithes.FirstOrDefault(t => t.Id == id)).FirstOrDefaultAsync();
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

        public async Task<List<Tithe>> All (string churchId)
        {
            try
            {
                var churchAndMemberExists = await _context.ChurchContext
                    .Where(c => c.Id == churchId)
                    .FirstOrDefaultAsync() is not null;
                if (!churchAndMemberExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return null;
                }
                var tithes = await _context.TitheContext
                    .Include(t => t.Member)
                    .Where(t => t.ChurchModelId == churchId)
                    .Select(t => (Tithe)t)
                    .ToListAsync();
                Console.WriteLine(tithes);
                return tithes;
            }
            catch
            {
                Notificator.SetNotificationType(new("Internal"));
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na listagem."));
                return null;
            }
        }

        public async Task<bool> Update (string churchId, string memberId, Tithe entity)
        {
            try
            {
                var churchAndMemberExists = await _context.ChurchContext
                    .Where(c => c.Id == churchId)
                    .Select(c => c.Members.FirstOrDefault(m => m.Id == memberId))
                    .FirstOrDefaultAsync() is not null;
                if (!churchAndMemberExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja ou membro não existe no sistema."));
                    return false;
                }

                var model = await _context.MemberContext.Where(m => m.Id == memberId)
                    .Select(m => m.Tithes.FirstOrDefault(t => t.Id == entity.Id)).FirstOrDefaultAsync();
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                model.Value = entity.Value;
                model.Date = entity.Date;
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

        public async Task<bool> Remove (string churchId, string memberId, string id)
        {
            try
            {
                var churchAndMemberExists = await _context.ChurchContext
                    .Where(c => c.Id == churchId)
                    .Select(c => c.Members.FirstOrDefault(m => m.Id == memberId))
                    .FirstOrDefaultAsync() is not null;
                if (!churchAndMemberExists)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Esta igreja ou membro não existe no sistema."));
                    return false;
                }

                var model = await _context.MemberContext.Where(m => m.Id == memberId)
                    .Select(m => m.Tithes.FirstOrDefault(t => t.Id == id)).FirstOrDefaultAsync();
                if (model is null)
                {
                    Notificator.SetNotificationType(new("NotFound"));
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                _context.TitheContext.Remove(model);
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
