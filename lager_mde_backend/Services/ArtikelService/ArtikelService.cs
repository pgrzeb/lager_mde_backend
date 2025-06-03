using lager_mde_backend.Data;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace lager_mde_backend.Services
{
    public class ArtikelService : IArtikelService
    {
        private readonly ApplicationDbContext _context;

        public ArtikelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetArtikelResponse> GetArtikelAsync(int artnr)
        {
            var artikel = await _context.Artikel
                .FirstOrDefaultAsync(a => a.artnr == artnr);

            if (artikel == null)
            {
                return new GetArtikelResponse
                {
                    nachricht = "Artikel nicht gefunden"
                };
            }

            if(artikel.block == 1)
            {
                return new GetArtikelResponse
                {
                    nachricht = "Fehler: keine palette vorhanden."
                };
            }

            return new GetArtikelResponse
            {
                art_id = artikel.art_id,
                artnr = artikel.artnr,
                artbez = artikel.artbez,
                lagerplatz = artikel.lagerplatz,
                anzahl_pal = artikel.anzahl_pal,
                block = artikel.block,
                durchl = artikel.durchl,
                nachricht = " "
            };

        }
    }
}