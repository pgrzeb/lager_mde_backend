using lager_mde_backend.Data;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class StapDisplayService : IStapDisplayService
    {
        private readonly ApplicationDbContext _context;

        public StapDisplayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetKomPlatzResponse> GetKomPlatz(GetKomPlatzRequest request)
        {
            string lagPl = "0";
            
            if (request.typ == 3)
            {
                var artikel = await _context.Artikel.FirstOrDefaultAsync(x => x.artnr == request.artnr);
                if (artikel != null) lagPl = artikel.lagerplatz;
            }

            return new GetKomPlatzResponse { lagerplatz = lagPl };
        }
        
        public async Task<GetLagPlatzResponse> GetLagLp(GetLagPlatzRequest request)
        {
            var lagstamm = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lagerplatz == request.lagerplatz);

            if (lagstamm == null)
                return new GetLagPlatzResponse { gefunden = false, nachricht = "Lagerplatz nicht gefunden!" };
            if(lagstamm != null && (lagstamm.artnr != 0 || lagstamm.sperre != 0))
                return new GetLagPlatzResponse { gefunden = false, nachricht = "Lagerplatz ist besetzt!" };
    
            return new GetLagPlatzResponse { gefunden = true };
        }


        public async Task<UpdateArtStapDisplayResponse> UpdateArtStapDisplayAsync(UpdateArtStapDisplayRequest request, string lagerplatz)
        {
            int j = 0;
            int m = 0;
            int t = 0;
            string z = "00:00";
            DateTime mh = DateTime.ParseExact("01.01.2000", "yyyy.MM.dd", null);

            var stapauf = await _context.Stapauf
                .FirstOrDefaultAsync(x => x.stap_id == request.stap_id);

            if (stapauf == null)
                return new UpdateArtStapDisplayResponse { nachricht = "Stapauf nicht gefunden!" };

            if (request.typ != 6)
                stapauf.status = 1;

            if (request.durchl == 1 && request.typ == 1)
                stapauf.restmeng = request.restmeng;

            if (request.typ == 3 && lagerplatz != null)
                stapauf.ziel = lagerplatz;

            if (request.typ != 6) {
                stapauf.status = 2;
                stapauf.benutzer = request.benutzer; // hier benutzer oder 0?
                stapauf.staufdat = DateTime.Now.ToString("yyyy.MM.dd");
                stapauf.staufend = DateTime.Now.ToString("HH:mm");
            }

            if ((request.typ == 4 || request.typ < 3) && request.von != "Bloc" && request.von != "R")
            {
                var lagstamm = await _context.Lagstamm
                    .FirstOrDefaultAsync(x => x.lagerplatz == request.von);

                if (lagstamm == null)
                {
                    return new UpdateArtStapDisplayResponse
                    {
                        nachricht = "Lagerplatz nicht gefunden!"
                    };
                }
                else
                {
                    lagstamm.sperre = 1;

                    j = lagstamm.jahr;
                    m = lagstamm.monat;
                    t = lagstamm.tag;
                    z = lagstamm.zeit;
                    mh = lagstamm.mhdatum;
                    stapauf.mhdatum = mh;

                    if (request.restmeng > 0)
                    {
                        if (lagerplatz != null && request.durchl == 1 && request.typ == 1)
                        {
                            var lagNeu = await _context.Lagstamm
                                .FirstOrDefaultAsync(x => x.lagerplatz == lagerplatz);
                            if (lagNeu == null)
                            {
                                return new UpdateArtStapDisplayResponse
                                {
                                    nachricht = "Lagerplatz nicht gefunden"
                                };
                            }

                            //neuer Lagerplatz für Restmenge
                            lagNeu.kisten = request.restmeng;
                            lagNeu.artnr = request.artnr;
                            lagNeu.sperre = 1;
                            lagNeu.jahr = j;
                            lagNeu.monat = m;
                            lagNeu.tag = t;
                            lagNeu.zeit = z;
                            lagNeu.mhdatum = mh;

                            //alter Lagerplatz wird genullt
                            lagstamm.artnr = 0;
                            lagstamm.kisten = 0;
                            lagstamm.sperre = 0;
                            lagstamm.jahr = 0;
                            lagstamm.monat = 0;
                            lagstamm.tag = 0;
                            lagstamm.zeit = "00:00";
                            lagstamm.mhdatum = DateTime.ParseExact("01.01.2000", "yyyy.MM.dd", null);

                            lagNeu.sperre = 0; //Datensatz wieder freigeben

                        }
                        else {
                            lagstamm.kisten = request.restmeng;
                            lagstamm.sperre = 0;
                        }
                    }
                    else
                    {
                        lagstamm.artnr = 0;
                        lagstamm.kisten = 0;
                        lagstamm.sperre = 0;
                        lagstamm.jahr = 0;
                        lagstamm.monat = 0;
                        lagstamm.tag = 0;
                        lagstamm.zeit = "00:00";
                        lagstamm.mhdatum = DateTime.ParseExact("01.01.2000", "yyyy.MM.dd", null);

                    }

                    stapauf.mhdatum = lagstamm.mhdatum;
                }
            }

            if (request.typ <= 5 && request.typ >= 3 && request.ziel != "Bloc" && request.ziel != "R")
            {
                var lagstamm = await _context.Lagstamm
                    .FirstOrDefaultAsync(x => x.lagerplatz == request.ziel);

                if (lagstamm == null)
                {
                    return new UpdateArtStapDisplayResponse
                    {
                        nachricht = "Lagerplatz nicht gefunden"
                    };
                }
                else
                {
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

                    if (request.restmeng > 0)
                    {
                        lagstamm.kisten = request.restmeng;
                        lagstamm.artnr = request.artnr;
                    }
                    else if (request.restmeng == 0 && request.menge > 0)
                    {
                        lagstamm.kisten = request.menge;
                    }
                }
            }

            var save = await _context.SaveChangesAsync();
            if (save == 0)
            {
                return new UpdateArtStapDisplayResponse
                {
                    nachricht = "Keine Änderungen vorgenommen!"
                };
            }

            return new UpdateArtStapDisplayResponse
            {
                nachricht = "Daten erfolgreich gespeichert!",
            };
        }
    }
}