using WebServiceCaixaEconomicaFederal.Model;

namespace WebServiceCaixaEconomicaFederal.ViewModel
{
    public class BoletoClientResult
    {
        public string LINHA_DIGITAVEL { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string NOSSO_NUMERO { get; set; }
        public string URL { get; set; }
        public string Operacao { get; set; }
        public string MENSAGENS { get; set; }
        public ConsultaBoletoSaida Consulta{ get; set; }
    }
}