using lager_mde_backend.Data;
using lager_mde_backend.Entities;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace lager_mde_backend.Services
{
    public class LeermeldService : ILeermeldService
    {
        private readonly ApplicationDbContext _context;

        public LeermeldService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<LeermeldArtResponse> GetLeermeldAsync(int artnr)
        {
            var artikel = await _context.Artikel
                .Where(a => a.artnr == artnr)
                .FirstOrDefaultAsync();

            var lagstamm = await _context.Lagstamm
                .Where(l => l.artnr == artnr && l.sperre == 0)
                .OrderBy(l => l.jahr)
                .ThenBy(l => l.monat)
                .ThenBy(l => l.tag)
                .FirstOrDefaultAsync(); 

            var stapauf = await _context.Stapauf
                .Where(s => s.artnr == artnr && s.typ == 1 && s.status < 2)
                .FirstOrDefaultAsync();

            string von1 = ""; 
            int meng = 0;

            if (artikel == null || lagstamm == null || stapauf != null) // stapauf != null bedeutet, dass bereits eine Leermeldung für diesen Artikel existiert
            {
                if (artikel == null)
                {
                    return new LeermeldArtResponse
                    {
                        nachricht = "Fehler: Artikel nicht gefunden!"
                    };
                }
                else if (lagstamm == null)
                {
                    if (artikel != null)
                    {
                        if (artikel.block == 1)
                        {
                            von1 = "Bloc";
                            meng = artikel.anzahl_pal;
                        }
                        else
                        {
                            return new LeermeldArtResponse
                            {
                                nachricht = "Fehler: Keine Palette vorhanden!"
                            };
                        }
                    } 
                    else 
                    {
                        return new LeermeldArtResponse
                        {
                            nachricht = "Fehler: Keine Palette vorhanden!"
                        };
                    }
                }
                else if (stapauf != null)
                {
                    return new LeermeldArtResponse
                    {
                        nachricht = "Leermeldung für diesen Artikel bereits vorhanden!"
                    };
                }
            }

            if (lagstamm != null)
            {
                von1 = lagstamm.lagerplatz;
                meng = lagstamm.kisten;
                lagstamm!.sperre = 1; 
                var save = await _context.SaveChangesAsync();

                if (save == 0)
                {
                    return new LeermeldArtResponse
                    {
                        nachricht = "Datensatz in Lagstamm gesperrt!"
                    };
                }
            }

            string ziel = artikel.lagerplatz;

            if (ziel.Contains("/"))
            {
                ziel = ziel.Split('/').Last();
            }

            var response = new LeermeldArtResponse
            {
                lag_id = lagstamm != null ? lagstamm.lag_id : 0,
                artnr = artikel.artnr,
                artbez = artikel.artbez,
                menge = meng,
                von = von1,
                ziel = ziel,
                durchl = artikel.durchl,
                nachricht = " "
            };

            await _context.SaveChangesAsync();

            if (lagstamm != null)
            {
                lagstamm.sperre = 0;
                await _context.SaveChangesAsync();
            }

            return response;
        }

        public async Task<UpdateLeermeldResponse> UpdateLeermeldAsync(UpdateLeermeldRequest request)
        {
            var stapauf = new Stapauf
            {
                artnr = request.artnr,
                artbez = request.artbez,
                von = request.von,
                ziel = request.ziel,
                menge = request.menge,
                datum =  DateTime.Now.ToString("dd.MM.yyyy"),
                typ = 1,
                status = 0,
                durchl = request.durchl,
                restmeng = 0,
                staufdat = "          ",
                staufanf = DateTime.Now.ToString("HH:mm"),
                staufend = "     ",
                benutzer = 0,
                user = request.komnr,
                mhd = 0,
            };
            _context.Stapauf.Add(stapauf);
            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {   
                return new UpdateLeermeldResponse
                {
                    nachricht = "Fehler: Datensatz konnte nicht gespeichert werden!"
                };
                
            }

            if (request.lag_id != null && save != 0)
            {
                var lagstamm = await _context.Lagstamm
                .FirstOrDefaultAsync(l => l.lag_id == request.lag_id);

                if (lagstamm != null){
                    if (lagstamm.sperre == 1)
                    {
                        return new UpdateLeermeldResponse
                        {
                            nachricht = "Fehler: Datensatz gesperrt!"
                        };
                    }
                    lagstamm.sperre = 1;
                    await _context.SaveChangesAsync();
                }
            }

            return new UpdateLeermeldResponse
            {
                nachricht = "Leermeldung erfolgreich gespeichert!"
            };
        }
    }
}