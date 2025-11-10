using lager_mde_backend.Data;
using lager_mde_backend.Entities;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class InventurService : IInventurService
    {
        private readonly ApplicationDbContext _context;

        public InventurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetInvArtResponse> GetArtAsync(int artnr)
        {
            var artikel = await _context.Artikel
                .Where(a => a.artnr == artnr)
                .FirstOrDefaultAsync();

            if (artikel == null)
            {
                return new GetInvArtResponse
                {
                    artnr = artnr,
                    nachricht = "Artikelnummer nicht gefunden!"
                };
            }

            var pruefDat = DateOnly.FromDateTime(DateTime.UtcNow);
            var inventur = await _context.Inventur.Where(i => i.artnr == artnr && i.datum == pruefDat).FirstOrDefaultAsync();

            if (inventur != null)
            {
                return new GetInvArtResponse
                {
                    artnr = artikel.artnr,
                    artbez = artikel.artbez,
                    nachricht = "Für diesen Artikel existiert bereits ein Eintrag für das heutige Datum."
                };
            }

            return new GetInvArtResponse
            {
                artnr = artikel.artnr,
                artbez = artikel.artbez,
            };
        }

        public async Task<InventurResponse> SaveInventurAsync(UpdateInventurRequest request)
        {
            var inventur = _context.Inventur;

            var inv = new Inventur
            {
                artnr = request.artnr,
                artbez = request.artbez,
                menge = request.menge,
                datum = DateOnly.FromDateTime(DateTime.UtcNow),
                benutzer = request.benutzer,
            };

            inventur.Add(inv);
            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {
                return new InventurResponse
            {
                nachricht = "Artikel konnte nicht gespeichert werden.",
            };
            }
            
            return new InventurResponse
            {
                nachricht = "Artikel erfolgreich gespeichert.",
            };

        } 
        
        public async Task<InventurResponse> UpdateMengeAsync(UpdateInventurRequest request)
        {
            var pruefDat = DateOnly.FromDateTime(DateTime.UtcNow);
            var inventur = await _context.Inventur.Where(i => i.artnr == request.artnr && i.datum == pruefDat).FirstOrDefaultAsync();

            if (inventur != null)
            {
                inventur.menge = request.menge;
                inventur.benutzer = request.benutzer;

                var save = await _context.SaveChangesAsync();

                if (save == 0)
                {
                    return new InventurResponse
                    {
                        nachricht = "Menge konnte nicht angepasst werden.",
                    };
                }

                return new InventurResponse
                {
                    nachricht = "Menge erfolgreich angepasst."
                };
            }

            return new InventurResponse
            {
                nachricht = "Menge konnte nicht angepasst werden.",
            };
        }
    }
}