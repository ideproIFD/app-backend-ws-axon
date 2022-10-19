namespace appbackendnetbankwebservice.Models.Dao
{
    public class ProximaCuotaCredito
    {
        public Int64 capital { get; set; }
        public Int64 diasMora { get; set; }
        public string estado { get; set; }
        public string fechaDeIncumplimiento { get; set; }
        public string moneda { get; set; }
        public decimal montoDesembolsado { get; set; }
        public decimal montoProximaCuota { get; set; }
        public string numeroCredito { get; set; }
        public string tipoCredito { get; set; }
        public List<HeaderWSAxon> header { get; set; }
    }
}
