using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    [XmlType(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public ResponseBody<SERVICO_ENTRADA> body { get; set; }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
        public SoapEnvelope()
        {
            xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            Header = "";
        }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class ResponseBody<T>
    {
        [XmlElement(Namespace = "http://caixa.gov.br/sibar/manutencao_cobranca_bancaria/boleto/externo")]
        public T SERVICO_ENTRADA { get; set; }

        [XmlElement(ElementName = "SERVICO_ENTRADA", Namespace = "http://caixa.gov.br/sibar/consulta_cobranca_bancaria/boleto")]
        public T SERVICO_ENTRADA_Consulta { get; set; }
    }
}