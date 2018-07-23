using System.Xml.Serialization;

namespace WebServiceCaixaEconomicaFederal.Model
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelopeSaida
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public ResponseBodySaida<SERVICO_SAIDA> body { get; set; }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public SoapEnvelopeSaida()
        {
            xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            Header = "";
        }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class ResponseBodySaida<T>
    {
        [XmlElement(Namespace = "http://caixa.gov.br/sibar/manutencao_cobranca_bancaria/boleto/externo")]
        public T SERVICO_SAIDA { get; set; }

        [XmlElement(ElementName = "SERVICO_SAIDA", Namespace = "http://caixa.gov.br/sibar/consulta_cobranca_bancaria/boleto")]
        public T SERVICO_SAIDA_Consulta { get; set; }
    }
}