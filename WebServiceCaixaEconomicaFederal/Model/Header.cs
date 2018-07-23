using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    [XmlRoot(Namespace = "")]
    public class HEADER
    {
        public string VERSAO { get; set; }
        public string AUTENTICACAO { get; set; }
        public string USUARIO_SERVICO { get; set; }
        public string OPERACAO { get; set; }        
        public string SISTEMA_ORIGEM { get; set; }
        public string DATA_HORA { get; set; }
    }
}