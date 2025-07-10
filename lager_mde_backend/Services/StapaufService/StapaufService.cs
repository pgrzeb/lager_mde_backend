using lager_mde_backend.Data;
using lager_mde_backend.Entities;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class StapaufService : IStapaufService
    {
        private readonly ApplicationDbContext _context;

        public StapaufService(ApplicationDbContext context)
        {
            _context = context;
        }

    public async Task<List<StapDisplayResponse>> GetStapDisplayAsync(int stapStatus)
        {
            var stapaufList = await _context.Stapauf
                .Where(x => x.status < stapStatus)
                .ToListAsync();

            var responseList = new List<StapDisplayResponse>();

            foreach (var stapauf in stapaufList)
            {
                responseList.Add(new StapDisplayResponse
                {
                    stap_id = stapauf.stap_id,
                    artbez = stapauf.artbez,
                    von = stapauf.von,
                    ziel = stapauf.ziel,
                    menge= stapauf.menge,
                    typ = stapauf.typ,
                    status = stapauf.status
                });
            }

            return responseList;
        }

        public async Task<GetStapaufResponse> GetStapaufAsync(int stapId)
        {
            var stapauf = await _context.Stapauf
                 .FirstOrDefaultAsync(x => x.stap_id == stapId);

            return new GetStapaufResponse
            {
                stap_id = stapauf!.stap_id,
                artnr = stapauf.artnr,
                artbez = stapauf.artbez,
                von = stapauf.von,
                ziel = stapauf.ziel,
                menge = stapauf.menge,
                typ = stapauf.typ,
                status = stapauf.status,
                durchl = stapauf.durchl,
                restmeng = (int)stapauf.restmeng,
                mhd = (int)stapauf.mhd,
                mhdatum = stapauf.mhdatum ?? DateTime.Parse("2012-01-01")
            };

        }

        public async Task<UpdateStapMengeResponse> UpdateStapMengeAsync(UpdateStapMengeRequest request)
        {
            var stapauf = await _context.Stapauf
                .FirstOrDefaultAsync(x => x.stap_id == request.stap_id);

            if ( request.lagerplatz != "")
            {
                var lagstamm = await _context.Lagstamm
                    .FirstOrDefaultAsync(x => x.lagerplatz == request.lagerplatz && x.sperre == 0);

                if (lagstamm == null)
                {
                    throw new Exception("Lagerplatz nicht gefunden oder gesperrt.");
                }
            }

            if (stapauf == null)
            {
                throw new Exception("Stapauf nicht gefunden.");
            }

            stapauf.restmeng = stapauf.menge - request.menge;
            stapauf.status = 2;
            stapauf.benutzer = request.benutzer;
            stapauf.staufdat = DateTime.Now.ToString("yyyy.MM.dd");
            stapauf.staufend = DateTime.Now.ToString("HH:mm");
            
            await _context.SaveChangesAsync();

            return new UpdateStapMengeResponse
            {
                stap_id = stapauf.stap_id,
                von = stapauf.von,
                typ = stapauf.typ,
                menge = stapauf.menge
            };
        }

        public async Task<UpdateStapaufResponse> UpdateStapMhdAsync(UpdateStapaufMhdRequest request, DateTime mhdatum)
        {
            var stapauf = await _context.Stapauf
                .FirstOrDefaultAsync(x => x.stap_id == request.stap_id);

            var lagstamm = await _context.Lagstamm
                .FirstOrDefaultAsync(x => x.lagerplatz == request.ziel && x.sperre == 0);

            if (stapauf == null)
            {

                return new UpdateStapaufResponse
                {
                    nachricht = "Status konnte nicht geändert werden."
                };
            }else if (lagstamm == null)
            {
                return new UpdateStapaufResponse
                {
                    nachricht = "Lagerplatz nicht gefunden oder gesperrt."
                };
            }

            if (stapauf.mhd == 1 && stapauf.typ != 4 )
            {
                stapauf.mhdatum = mhdatum;
                stapauf.status = 2;
                lagstamm.mhdatum = mhdatum;
                await _context.SaveChangesAsync();
                return new UpdateStapaufResponse
                {
                    nachricht = "MHD erfolgreich geändert"
                };
            }
            else
            {
                stapauf.status = 2;
                await _context.SaveChangesAsync();
                return new UpdateStapaufResponse
                {
                    nachricht = "Auftrag erfolgreich abgeschlossen"
                };
            }
        }


        public async Task<UpdateStapaufResponse> UpdateStapaufAsync(UpdateStapaufRequest request)
        {
            var stapauf = new Stapauf
            {
                artnr = request.artnr,
                artbez = request.artbez,
                von = request.von,
                ziel = request.ziel,
                menge = request.menge,
                datum = request.datum,
                typ = 1,
                status = 0,
                durchl = request.durchl,
                restmeng = 0,
                staufdat = "          ",
                staufanf = request.staufanf,
                staufend = "     ",
                benutzer = 0,
                user = 0,
                mhd = 0,
            };
            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {
                return new UpdateStapaufResponse
                {
                    nachricht = "Fehler beim speichern der Leermeldung"
                };
            }

            return new UpdateStapaufResponse
            {
                nachricht = "Leermeldung erfolgreich gespeichert"
            };
        }
    }
}