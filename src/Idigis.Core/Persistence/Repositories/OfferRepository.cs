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
    internal class OfferRepository : IOfferRepository
    {
        private readonly Context _context;

        public OfferRepository (AbstractNotificator notificator, Context context)
        {
            Notificator = notificator;
            _context = context;
        }

        public AbstractNotificator Notificator { get; }

        public async Task<bool> Add (string churchId, Offer entity)
        {
            OfferModel model = entity;
            try
            {
                var churchExists =
                    await _context.ChurchContext.FirstOrDefaultAsync(church => church.Id == churchId) is not null;
                if (!churchExists)
                {
                    Notificator.SetNotificationType(new("Validation"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                model.ChurchId = churchId;
                var entry = await _context.OfferContext.AddAsync(model);
                await _context.SaveChangesAsync();
                return entry.State is EntityState.Unchanged;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na inserção."));
                return false;
            }
        }

        public async Task<Offer> GetById (string churchId, string id)
        {
            try
            {
                var offers = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Offers)
                    .FirstOrDefaultAsync();
                if (offers is null)
                {
                    Notificator.SetNotificationType(new("Validation"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return null;
                }

                var model = offers.Find(offer => offer.Id == id);
                if (model is null)
                {
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                }

                return model;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na busca."));
                return null;
            }
        }

        public async Task<List<Offer>> All (string churchId)
        {
            try
            {
                var offers = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Offers.Select(o => (Offer)o).ToList())
                    .FirstOrDefaultAsync();
                if (offers is not null)
                {
                    return offers;
                }

                Notificator.SetNotificationType(new("Validation"));
                Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                return null;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na listagem."));
                return null;
            }
        }

        public async Task<bool> Update (string churchId, Offer entity)
        {
            try
            {
                var offers = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Offers)
                    .FirstOrDefaultAsync();
                if (offers is null)
                {
                    Notificator.SetNotificationType(new("Validation"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                var model = offers.Find(offer => offer.Id == entity.Id);
                if (model is null)
                {
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                model.Value = entity.Value;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na atualização."));
                return false;
            }
        }

        public async Task<bool> Remove (string churchId, string id)
        {
            try
            {
                var offers = await _context.ChurchContext.Where(c => c.Id == churchId)
                    .Select(c => c.Offers)
                    .FirstOrDefaultAsync();
                if (offers is null)
                {
                    Notificator.SetNotificationType(new("Validation"));
                    Notificator.AddNotification(new("Repository", "Esta igreja não existe no sistema."));
                    return false;
                }

                var model = offers.Find(offer => offer.Id == id);
                if (model is null)
                {
                    Notificator.AddNotification(new("Repository", "Registro não encontrado."));
                    return false;
                }

                _context.OfferContext.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Notificator.AddNotification(new("Repository", "Ocorreu um erro na remoção."));
                return false;
            }
        }
    }
}
