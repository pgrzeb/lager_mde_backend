namespace lager_mde_backend.Models
{
    public class UpdateStapMengeResponse
    {
        public int stap_id { get; set; }
        public required string von { get; set; }
        public int typ { get; set; }
        public int menge { get; set; }
    }
}