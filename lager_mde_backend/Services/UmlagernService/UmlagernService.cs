using lager_mde_backend.Data;
using lager_mde_backend.Entities;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class UmlagernService : IUmlagernService
    {
        private readonly ApplicationDbContext _context;

        public UmlagernService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetLagPlatzIdResponse> GetLagPlatzIdAsync(GetLagPlatzIdRequest request)
        {
            var lagstamm = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lagerplatz == request.lagerplatz);

            if (lagstamm == null)
                return new GetLagPlatzIdResponse { gefunden = false, nachricht = "Lagerplatz nicht gefunden!" };
            else
            {
                if (lagstamm.artnr == 0 && lagstamm.sperre == 0 && !request.vonLag)
                {

                    return new GetLagPlatzIdResponse { gefunden = true, lag_id = lagstamm.lag_id };
                }
                else if( lagstamm.sperre == 0 && request.vonLag)
                {
                    return new GetLagPlatzIdResponse { gefunden = true, lag_id = lagstamm.lag_id};
                }
                else
                {
                    return new GetLagPlatzIdResponse { gefunden = false, nachricht = "Lagerplatz ist besetzt!" };
                }
            }
        }

        public async Task<UpdateLagPlatzResponse> UpdateArtLagPlatzAsync(UpdateLagPlatzRequest request)
        {

            var lagNach = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lag_id == request.nachId);
            var lagVon = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lag_id == request.vonId);

            if (lagVon == null || lagNach == null)
            {
                return new UpdateLagPlatzResponse { umlagern = false, nachricht = "Ein Lagerplatz konnte nicht gesperrt werden!" };
            }

            lagNach.sperre = 1;
            lagVon.sperre = 1;

            var stapauf = new Stapauf
            {
                artnr = request.artnr,
                artbez = request.artbez,
                von = request.von,
                ziel = request.ziel,
                menge = request.menge,
                datum = DateTime.Now.ToString("dd.MM.yyyy"),
                typ = 4,
                status = 0,
                durchl = 0,
                restmeng = 0,
                staufdat = "          ",
                staufanf = DateTime.Now.ToString("HH:mm"),
                staufend = "     ",
                benutzer = 0,
                user = 0,
                mhd = request.mhdatum.Year > 2014 ? 1 : 0,
                mhdatum = DateTime.SpecifyKind(request.mhdatum, DateTimeKind.Utc),
            };
            _context.Stapauf.Add(stapauf);
            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {   
                return new UpdateLagPlatzResponse { umlagern = false, nachricht = "Die Umlagerung konnte nicht erstellt werden!" };
            }

            return new UpdateLagPlatzResponse { umlagern = true };
        }

        public async Task<GetLagPlatzVonResponse> GetLagPlatzVonAsync(int id)
        {
            var lagstamm = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lag_id == id);
 
            if (lagstamm != null)
            {
                var artikel = await _context.Artikel.FirstOrDefaultAsync(x => x.artnr == lagstamm.artnr);
                return new GetLagPlatzVonResponse
                {
                    artnr = lagstamm.artnr,
                    artbez = artikel == null ? " " : artikel.artbez,
                    menge = lagstamm.kisten,
                    tag = lagstamm.tag,
                    monat = lagstamm.monat,
                    jahr = lagstamm.jahr,
                    zeit = lagstamm.zeit,
                    mhdatum = DateTime.SpecifyKind(lagstamm.mhdatum ?? DateTime.Parse("2000-01-01"), DateTimeKind.Utc),
                };
            }
            else
            {
                return new GetLagPlatzVonResponse
                {
                    artnr = 0,
                    artbez = " ",
                    menge = 0,
                    tag = 0,
                    monat = 0,
                    jahr = 0,
                    zeit = " ",
                    mhdatum = DateTime.SpecifyKind(DateTime.Parse("2000-01-01"), DateTimeKind.Utc),
                };
            }

        }

    }
}