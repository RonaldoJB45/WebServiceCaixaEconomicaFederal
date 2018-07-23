namespace WebServiceCaixaEconomicaFederal.Model
{
    public class Pagamento
    {
        public short QUANTIDADE_PERMITIDA { get; set; }
        public string TIPO { get; set; }

        public string VALOR_MINIMO { get; set; }
        public bool ShouldSerializeVALOR_MINIMO()
        {
            return !string.IsNullOrEmpty(this.VALOR_MINIMO);
        }

        public string VALOR_MAXIMO { get; set; }
        public bool ShouldSerializeVALOR_MAXIMO()
        {
            return !string.IsNullOrEmpty(this.VALOR_MAXIMO);
        }

        public string PERCENTUAL_MINIMO { get; set; }
        public bool ShouldSerializePERCENTUAL_MINIMO()
        {
            return !string.IsNullOrEmpty(this.PERCENTUAL_MINIMO);
        }

        public string PERCENTUAL_MAXIMO { get; set; }
        public bool ShouldSerializePERCENTUAL_MAXIMO()
        {
            return !string.IsNullOrEmpty(this.PERCENTUAL_MAXIMO);
        }
    }
}