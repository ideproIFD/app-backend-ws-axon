using appbackendnetbankwebservice.Models.Dao;

namespace appbackendnetbankwebservice.Models
{
    public class IdeProximaCuotaCreditoResponse : IdeResponse
    {
        public List<ProximaCuotaCredito> rows { get; set; }
        public Int64 total { get; set; }
    }
}
