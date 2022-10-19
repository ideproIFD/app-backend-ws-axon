namespace appbackendnetbankwebservice.Models
{
    public class IdeDatosClienteResponse :IdeResponse
    {
        public List<DatosCliente> rows { get; set; }
        public Int64 total { get; set; }
    }
}
