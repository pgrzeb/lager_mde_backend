using lager_mde_backend.Data;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class LagstammService : ILagstammService
    {
        private readonly ApplicationDbContext _context;

        public LagstammService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateLagstammPlatzResponse> LagstammPlatzResponseAsync(UpdateLagstammPlatzRequest request, String lagerplatz)
        {
            var lagstamm = await _context.Lagstamm
                .FirstOrDefaultAsync(x => x.lagerplatz == lagerplatz);

            if (lagstamm == null || request == null)
            {
                lagstamm = await _context.Lagstamm.FirstOrDefaultAsync(XmlConfigurationExtensions => XmlConfigurationExtensions.lagerplatz == "BLOC");
                if (lagstamm == null)
                {
                    return new UpdateLagstammPlatzResponse
                    {
                        nachricht = "Lagerplatz nicht gefunden"
                    };
                }
            }

            int j = lagstamm.jahr;
            int m = lagstamm.monat;
            int t = lagstamm.tag;
            string z = lagstamm.zeit;
            DateTime mh = lagstamm.mhdatum ;

            if ((request!.typ == 4 || request.typ < 3) && request.von != "Bloc" && request.von != "R")
            {
                
                if (request.restmeng > 0)
                {
                    lagstamm.kisten = request.restmeng;
                } else if (request.restmeng != request.menge && request.restmeng < request.menge)
                {
                    lagstamm.kisten = request.menge - request.restmeng;
                }
                else if (request.restmeng == 0 && request.menge > 0)
                {
                    lagstamm.kisten = request.menge;
                }
                else
                {
                    lagstamm.artnr = 0;
                    lagstamm.kisten = 0;
                    lagstamm.jahr = 0;
                    lagstamm.monat = 0;
                    lagstamm.tag = 0;
                    lagstamm.zeit = "     ";
                    lagstamm.mhdatum = DateTime.ParseExact("01.01.2012", "yyyy.MM.dd", null);
                }

            } 
            
            if ((request.typ == 5 || request.typ == 4 || request.typ == 3) && request.ziel != "Bloc" && request.ziel != "R")
            {
                lagstamm.artnr = request.artnr;
                lagstamm.kisten = request.menge;

                if (request.typ == 4)
                {
                    lagstamm.jahr = j;
                    lagstamm.monat = m;
                    lagstamm.tag = t;
                    lagstamm.zeit = z;
                    lagstamm.mhdatum = mh;
                }
                else if (request.typ == 5)
                {
                    lagstamm.jahr = 2000;
                    lagstamm.monat = 1;
                    lagstamm.tag = 1;
                    lagstamm.zeit = "00:00";
                } 
                else 
                {
                    lagstamm.jahr = DateTime.Now.Year;
                    lagstamm.monat = DateTime.Now.Month;
                    lagstamm.tag = DateTime.Now.Day;
                    lagstamm.zeit = DateTime.Now.ToString("HH:mm");
                }
            }

            lagstamm.sperre = 0;
            var save = await _context.SaveChangesAsync();
            if (save == 0)
            {
                return new UpdateLagstammPlatzResponse
                {
                    mhdatum = lagstamm.mhdatum,
                    nachricht = "Keine Änderungen in Lagstamm vorgenommen"
                };
            }

            return new UpdateLagstammPlatzResponse
            {
                mhdatum = lagstamm.mhdatum,
                nachricht = " ",
            };
        }

        public async Task<LagstammArtikelResponse> GetLagstammArtikelAsync(int artnr){
            var lagstamm = await _context.Lagstamm
                .FirstOrDefaultAsync(x => x.artnr == artnr && x.sperre == 0);

            if (lagstamm == null)
            {
                return new LagstammArtikelResponse
                {
                    nachricht = "Artikel nicht gefunden"
                };
            }

            string von1 = lagstamm.lagerplatz;
            int meng = lagstamm.kisten;

            lagstamm.sperre = 1; 

            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {
                return new LagstammArtikelResponse
                {
                    nachricht = "Fehler beim speichern in Lagstamm"
                };
            }
          
            return new LagstammArtikelResponse
            {
                artnr = lagstamm.artnr,
                sperre = lagstamm.sperre,
                lagplatz = von1,
                lagMeng = meng,
                nachricht = " "
            };
        }

        public async Task<UpdateLagstammStatusResponse> UpdateLagstammStatusAsync(int lagId)
        {
            var lagstamm = await _context.Lagstamm
                .FirstOrDefaultAsync(x => x.lag_id == lagId );

            if (lagstamm == null)
            {
                return new UpdateLagstammStatusResponse
                {
                    nachricht = "Lagstamm nicht gefunden"
                };
            }
            
            lagstamm.sperre = 0;
            var save = await _context.SaveChangesAsync();

            if (save == 0)
            {
                return new UpdateLagstammStatusResponse
                {
                    nachricht = "Fehler beim speichern in Lagstamm"
                };
            }

            return new UpdateLagstammStatusResponse
            {
                sperre = lagstamm.sperre,
            };
        }
    }
}