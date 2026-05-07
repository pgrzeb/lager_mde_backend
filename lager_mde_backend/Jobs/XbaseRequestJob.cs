public class XbaseRequestJob
{
    public int ArtikelId { get; set; }
    // Das ist das "Versprechen", das Ergebnis später zu liefern
    public TaskCompletionSource<string> tcs { get; set; } = new();
}