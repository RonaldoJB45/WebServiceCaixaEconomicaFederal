using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    public class MENSAGEM
    {
        [XmlElement(ElementName = "MENSAGEM")]
        public string descricaoMensagem { get; set; }
        public string RETORNO { get; set; }
    }
}