using lager_mde_backend.Models;

namespace lager_mde_backend.Jobs;
public class XbaseRequestJob
{
    public int ArtikelId { get; set; }
    // Das ist das "Versprechen", das Ergebnis später zu liefern
    public TaskCompletionSource<GetInvArtResponse> tcs { get; set; } = new();
}