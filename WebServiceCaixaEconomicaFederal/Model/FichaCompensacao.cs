using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    public class FichaCompensacao
    {
        [XmlArray("MENSAGENS")]
        [XmlArrayItem(ElementName = "MENSAGEM")]
        public List<string> MENSAGENS { get; set; }
    }
}