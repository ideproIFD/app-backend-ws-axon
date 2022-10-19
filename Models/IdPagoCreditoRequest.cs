namespace appbackendnetbankwebservice.Models
{
    public class IdePagoCreditoRequest
    {
        public int canal { get; set; }
        public string codigo_cliente { get; set; }
        public string codigo_apoderado { get; set; }
        public string numero_cc { get; set; }
        public string numero_credito { get; set; }
        public string fecha { get; set; }
        public string moneda { get; set; }
        public string signo_1 { get; set; }
        public string monto_pago { get; set; }

    }
}
