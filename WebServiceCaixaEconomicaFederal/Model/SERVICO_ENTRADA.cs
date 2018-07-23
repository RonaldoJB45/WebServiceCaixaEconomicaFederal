using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    [XmlRoot(Namespace = "")]
    public class SERVICO_ENTRADA
    {
        [XmlElement(Namespace = "http://caixa.gov.br/sibar")]
        public HEADER HEADER { get; set; }
        [XmlElement(Namespace = "")]
        public DADOS DADOS { get; set; }
    }
}