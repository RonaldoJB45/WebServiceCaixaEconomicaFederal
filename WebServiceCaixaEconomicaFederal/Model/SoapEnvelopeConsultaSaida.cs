using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelopeConsultaSaida
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public ResponseBodyConsultaSaida<SERVICO_SAIDA_CONSULTA> body { get; set; }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public SoapEnvelopeConsultaSaida()
        {
            xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            Header = "";
        }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class ResponseBodyConsultaSaida<T>
    {
        [XmlElement(ElementName = "SERVICO_SAIDA", Namespace = "http://caixa.gov.br/sibar/consulta_cobranca_bancaria/boleto")]
        public T SERVICO_SAIDA { get; set; }
    }
}