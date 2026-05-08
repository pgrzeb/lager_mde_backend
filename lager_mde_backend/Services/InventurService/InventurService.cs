using lager_mde_backend.Data;
using lager_mde_backend.Entities;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;

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
            /*var artikel = await _context.Artikel
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
                    nachricht = "Für diesen Artikel existiert bereits ein Eintrag für das heutige Datum.",
                    menge = inventur.menge,
                };
            }

            return new GetInvArtResponse                  
            {                                               
                artnr = artikel.artnr,                     
                artbez = artikel.artbez,                    
            }; 
            */
            using var client = new HttpClient();

            var response = await client.GetAsync($"http://192.168.125.111:8080/inventur/getArt?artnr={artnr}");
            
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<GetInvArtResponse>(jsonString) ?? new GetInvArtResponse                  
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            };
            return content;
        }

        public async Task<InventurResponse> SaveInventurAsync(UpdateInventurRequest request)
        {
            /*var inventur = _context.Inventur;

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
            };*/

            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(request);

            // JSON in HTTP-Content packen
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "http://192.168.125.111:8080/inventur/save",
                content
            );
            
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var returnContent = JsonSerializer.Deserialize<InventurResponse>(jsonString) ?? new  InventurResponse                 
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            };
            return returnContent;

        } 
        
        public async Task<InventurResponse> UpdateMengeAsync(UpdateInventurRequest request)
        {
           /* var pruefDat = DateOnly.FromDateTime(DateTime.UtcNow);
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
            }; */

            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(request);

            // JSON in HTTP-Content packen
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                "http://192.168.125.111:8080/inventur/update",
                content
            );
            
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var returnContent = JsonSerializer.Deserialize<InventurResponse>(jsonString) ?? new  InventurResponse                 
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            };
            return returnContent;
        }

        public async Task<List<GetInventurResponse>> GetInventurAsync()
        {
            var pruefDat = DateOnly.FromDateTime(DateTime.UtcNow);
            var eintraege = await _context.Inventur.ToListAsync();
            List<GetInventurResponse> inventur = [];

            if (eintraege != null)
            {
                foreach (var e in eintraege)
                {
                    inventur.Add(new GetInventurResponse{ artnr = e.artnr, artbez = e.artbez ?? "", menge = e.menge, datum = e.datum, benutzer = e.benutzer});
                }
                return inventur;
            } else
            {
                inventur.Add(new GetInventurResponse { artnr = 0, artbez = "", menge = 0, datum = pruefDat, benutzer = 0, nachricht = "Inventureintraege konnetn nicht exportiert werden" });
                return inventur;
            }
            
        }
    }
}