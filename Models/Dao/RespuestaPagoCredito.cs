namespace appbackendnetbankwebservice.Models.Dao
{
    public class RespuestaPagoCredito
    {
        public string numero_transaccion { get; set; }
        public string fecha { get; set; }
        public string signo_1 { get; set; }
        public string saldo_despues_pago { get; set; }

        // Para el caso de error
        public string error { get; set; }
        public string mensaje { get; set; }
    }
}
