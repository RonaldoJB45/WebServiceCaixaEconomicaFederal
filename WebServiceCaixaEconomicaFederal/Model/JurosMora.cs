namespace WebServiceCaixaEconomicaFederal.Model
{
    public class JurosMora
    {
        public string TIPO { get; set; }
        public string DATA { get; set; }

        public decimal? VALOR { get; set; }
        public bool ShouldSerializeVALOR()
        {
            return this.VALOR.HasValue;
        }

        public decimal? PERCENTUAL { get; set; }
        public bool ShouldSerializePERCENTUAL()
        {
            return this.PERCENTUAL.HasValue;
        }
    }
}