using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    public class SERVICO_SAIDA_CONSULTA
    {
        [XmlElement(Namespace = "http://caixa.gov.br/sibar")]
        public HEADER HEADER { get; set; }

        public string COD_RETORNO { get; set; }
        public string ORIGEM_RETORNO { get; set; }
        public string MSG_RETORNO { get; set; }

        [XmlElement(Namespace = "")]
        public DadosSaida DADOS { get; set; }
    }
}