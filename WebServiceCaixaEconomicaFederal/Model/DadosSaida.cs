namespace WebServiceCaixaEconomicaFederal.Model
{
    public class DadosSaida
    {
        public CONTROLE_NEGOCIAL CONTROLE_NEGOCIAL { get; set; }
        public IncluiBoletoSaida INCLUI_BOLETO { get; set; }
        public ConsultaBoletoSaida CONSULTA_BOLETO { get; set; }
        public AlteraBoletoSaida ALTERA_BOLETO { get; set; }
    }
}