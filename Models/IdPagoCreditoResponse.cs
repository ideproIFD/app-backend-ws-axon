using appbackendnetbankwebservice.Models.Dao;

namespace appbackendnetbankwebservice.Models
{
    public class IdePagoCreditoResponse :IdeResponse
    {
        public List<RespuestaPagoCredito> rows { get; set; }
    }
}
