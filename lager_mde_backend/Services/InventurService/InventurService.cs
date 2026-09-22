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
         private readonly IXBaseService _xbase;

        public InventurService(ApplicationDbContext context, IXBaseService xbase)
        {
            _context = context;
            _xbase = xbase;
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

            /*using var client = new HttpClient();

            var response = await client.GetAsync($"http://192.168.125.111:8080/inventur/getArt?artnr={artnr}");
            
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync(); var content = JsonSerializer.Deserialize<GetInvArtResponse>(jsonString) ?? new GetInvArtResponse                  
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            };*/

             var content = await _xbase.GetAsync<GetInvArtResponse>($"http://192.168.125.111:8080/inventur/getArt?artnr={artnr}") ?? new GetInvArtResponse                  
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            };

            if (content.artbez != null) content.artbez = artikel.artbez;

            return content;
        }

        public async Task<InventurResponse> SaveInventurAsync(UpdateInventurRequest request)
        {
            //using var client = new HttpClient();
            /*var json = JsonSerializer.Serialize(request);

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
            }; */

            var returnContent = await _xbase.PostAsync<InventurResponse>("http://192.168.125.111:8080/inventur/save",request) ??  new  InventurResponse                 
            {                                               
                nachricht = "Etwas ist schief gelaufen."                    
            }; 
            
            return returnContent;
        } 
        
        public async Task<InventurResponse> UpdateMengeAsync(UpdateInventurRequest request)
        {
            /*using var client = new HttpClient();
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
            return returnContent; */

             var returnContent = await _xbase.PostAsync<InventurResponse>("http://192.168.125.111:8080/inventur/update",request) ??  new  InventurResponse                 
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
                inventur.Add(new GetInventurResponse { artnr = 0, artbez = "", menge = 0, datum = pruefDat, benutzer = 0, nachricht = "Inventureintraege konnten nicht exportiert werden" });
                return inventur;
            }
        }
    }
}