using lager_mde_backend.Data;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace lager_mde_backend.Services
{
    public class ArtDisplayService : IArtDisplayService
    {
        private readonly ApplicationDbContext _context;

        public ArtDisplayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetKomPlatzResponse> GetKomPlatzAsync(int artnr)
        {
            string lagPl = "0";
            
            var artikel = await _context.Artikel.FirstOrDefaultAsync(x => x.artnr == artnr);
            if (artikel != null) lagPl = artikel.lagerplatz;

            return new GetKomPlatzResponse { lagerplatz = lagPl };
        }
        
        public async Task<GetLagPlatzIdResponse> GetLagPlatzAsync(string lagerplatz)
        {
            var lagstamm = await _context.Lagstamm.FirstOrDefaultAsync(x => x.lagerplatz == lagerplatz);

            if (lagstamm == null)
                return new GetLagPlatzIdResponse { gefunden = false, nachricht = "Lagerplatz nicht gefunden!" };
            if (lagstamm != null)
            {
                if (lagstamm.artnr != 0)
                {
                    return new GetLagPlatzIdResponse { gefunden = false, nachricht = "Lagerplatz ist besetzt!" };
                }
                else if (lagstamm.sperre != 0)
                {
                    return new GetLagPlatzIdResponse { gefunden = false, nachricht = "Lagerplatz ist gesperrt!" };
                }
            }   
    
            return new GetLagPlatzIdResponse { gefunden = true };
        }


        public async Task<UpdateArtStapDisplayResponse> UpdateArtStapDisplayAsync(UpdateArtStapDisplayRequest request)
        {
            int j = 0;
            int m = 0;
            int t = 0;
            string z = "00:00";
            DateTime defaultDate = DateTime.SpecifyKind(DateTime.Parse("2000-01-01"), DateTimeKind.Utc);
            DateTime mh = defaultDate;

            var stapauf = await _context.Stapauf
                .FirstOrDefaultAsync(x => x.stap_id == request.stap_id);

            if (stapauf == null)
                return new UpdateArtStapDisplayResponse { nachricht = "Stapauf nicht gefunden!" };

            if (request.typ != 6)
                stapauf.status = 1;

            if (request.durchl == 1 && request.typ == 1)
                stapauf.restmeng = request.restmeng;

            if (request.typ == 3 && request.lagerplatz != "0") //wenn Kommissionierplatz als Lagerplatz gewählt
                stapauf.ziel = request.lagerplatz;

            if (request.typ != 6) {
                stapauf.status = 2;
                stapauf.benutzer = request.benutzer; 
                stapauf.staufdat = DateTime.Now.ToString("dd.MM.yyyy");
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
                    mh = lagstamm.mhdatum ?? DateTime.SpecifyKind(DateTime.Parse("2000-01-01"), DateTimeKind.Utc);

                    if (request.restmeng > 0)
                    {
                        if (request.lagerplatz != "0" && request.durchl == 1 && request.typ == 1)
                        {
                            var lagNeu = await _context.Lagstamm
                                .FirstOrDefaultAsync(x => x.lagerplatz == request.lagerplatz);
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
                            lagNeu.mhdatum = DateTime.SpecifyKind(mh, DateTimeKind.Utc);

                            //alter Lagerplatz wird genullt
                            lagstamm.artnr = 0;
                            lagstamm.kisten = 0;
                            lagstamm.sperre = 0;
                            lagstamm.jahr = 0;
                            lagstamm.monat = 0;
                            lagstamm.tag = 0;
                            lagstamm.zeit = "00:00";
                            lagstamm.mhdatum = DateTime.SpecifyKind(DateTime.Parse("2000-01-01"), DateTimeKind.Utc);;

                            lagNeu.sperre = 0; //Datensatz wieder freigeben

                        }
                        else {
                            lagstamm.artnr = request.artnr;
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
                        lagstamm.mhdatum = DateTime.SpecifyKind(DateTime.Parse("2000-01-01"), DateTimeKind.Utc);

                    }
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
                        lagstamm.mhdatum = DateTime.SpecifyKind(mh, DateTimeKind.Utc);
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
                    }
                    else if (request.restmeng == 0 && request.menge > 0)
                    {
                        lagstamm.kisten = request.menge;
                    }
                    lagstamm.artnr = request.artnr;
                    lagstamm.sperre = 0;
                        
                    if (request.mhdatum != null  && request.typ != 4)
                    {
                        var mhdatumToCompare = DateTime.SpecifyKind(request.mhdatum.Value, DateTimeKind.Utc);
                        if (mhdatumToCompare != defaultDate)
                        {
                            lagstamm.mhdatum = DateTime.SpecifyKind(request.mhdatum.Value, DateTimeKind.Utc);
                            stapauf.mhdatum = DateTime.SpecifyKind(request.mhdatum.Value, DateTimeKind.Utc);
                        }
                    }
                    else
                    {
                        stapauf.mhdatum = DateTime.SpecifyKind(mh, DateTimeKind.Utc);
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