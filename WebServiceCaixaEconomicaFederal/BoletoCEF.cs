using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WebServiceCaixaEconomicaFederal.Model;
using WebServiceCaixaEconomicaFederal.Util;
using WebServiceCaixaEconomicaFederal.ViewModel;

namespace WebServiceCaixaEconomicaFederal
{
    public class BoletoCEF
    {
        public BoletoClientResult ManutencaoBoleto(BoletoClient boletoClient)
        {
            try
            {
                //cria xml conforme manual http://www.caixa.gov.br/Downloads/cobranca-caixa/Manual_Leiaute_Webservice.pdf
                string _xmlBoleto = GeraXmlBoleto(boletoClient);
                XmlDocument document = new XmlDocument();
                document.LoadXml(_xmlBoleto);

                string _operacao = "CONSULTA_BOLETO";
                string _endPoint = "https://barramento.caixa.gov.br/sibar/ConsultaCobrancaBancaria/Boleto";

                switch (boletoClient.Operacao)
                {
                    case Operacao.INCLUI_BOLETO:
                        _operacao = "INCLUI_BOLETO";
                        _endPoint = "https://barramento.caixa.gov.br/sibar/ManutencaoCobrancaBancaria/Boleto/Externo";
                        break;
                    case Operacao.ALTERA_BOLETO:
                        _operacao = "ALTERA_BOLETO";
                        _endPoint = "https://barramento.caixa.gov.br/sibar/ManutencaoCobrancaBancaria/Boleto/Externo";
                        break;
                    default:
                        break;
                }

                string _xmlRetorno = UtilHelper.CallWebRequest(_endPoint, document.InnerXml, _operacao);
                var retorno = XmlHelper.Deserialize<SoapEnvelopeSaida>(_xmlRetorno);

                BoletoClientResult result = new BoletoClientResult();

                DadosSaida saida = null;

                if (boletoClient.Operacao == Operacao.CONSULTA_BOLETO && retorno.body.SERVICO_SAIDA_Consulta.DADOS.CONSULTA_BOLETO == null)
                {
                    return null;
                }
                else
                {
                    if(boletoClient.Operacao == Operacao.CONSULTA_BOLETO)
                    {
                        saida = retorno.body.SERVICO_SAIDA_Consulta.DADOS;
                    }
                    else
                    {
                        saida = retorno.body.SERVICO_SAIDA.DADOS;
                    }

                    switch (boletoClient.Operacao)
                    {
                        case Operacao.INCLUI_BOLETO:
                            result.LINHA_DIGITAVEL = saida.INCLUI_BOLETO.LINHA_DIGITAVEL;
                            result.CODIGO_BARRAS = saida.INCLUI_BOLETO.CODIGO_BARRAS;
                            result.NOSSO_NUMERO = (!string.IsNullOrEmpty(saida.INCLUI_BOLETO.NOSSO_NUMERO) && saida.INCLUI_BOLETO.NOSSO_NUMERO != "0") ? saida.INCLUI_BOLETO.NOSSO_NUMERO : boletoClient.NossoNumero.ToString();
                            result.URL = saida.INCLUI_BOLETO.URL;
                            result.Operacao = "INCLUI_BOLETO";
                            break;
                        case Operacao.ALTERA_BOLETO:
                            result.LINHA_DIGITAVEL = saida.ALTERA_BOLETO.LINHA_DIGITAVEL;
                            result.CODIGO_BARRAS = saida.ALTERA_BOLETO.CODIGO_BARRAS;
                            result.NOSSO_NUMERO = (!string.IsNullOrEmpty(saida.ALTERA_BOLETO.NOSSO_NUMERO) || saida.ALTERA_BOLETO.NOSSO_NUMERO == "0") ? saida.ALTERA_BOLETO.NOSSO_NUMERO : boletoClient.NossoNumero.ToString();
                            result.URL = saida.ALTERA_BOLETO.URL;
                            result.Operacao = "ALTERA_BOLETO";
                            break;
                        default:
                            _operacao = "CONSULTA_BOLETO";
                            result.URL = saida.CONSULTA_BOLETO.TITULO.URL;
                            result.Consulta = saida.CONSULTA_BOLETO;
                            break;
                    }

                    return result;
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        private string GerarHash(BoletoClient boletoClient)
        {
            string codigoBeneficiario = boletoClient.CodigoBeneficiario.PadLeft(7, '0');
            string nossoNumero = boletoClient.NossoNumero.ToString().PadLeft(17, '0');
            string dataVencimento = boletoClient.Operacao == Operacao.CONSULTA_BOLETO ? "00000000" : boletoClient.DataVencimento.ToString("ddMMyyyy");
            string valor = boletoClient.Operacao == Operacao.CONSULTA_BOLETO ? "000000000000000" : boletoClient.Valor.ToString("n2").Replace(".", "").Replace(",", "").PadLeft(15, '0');
            string documento = boletoClient.BeneficiarioDocumento.PadLeft(14, '0');
            string valoresAutenticacao = codigoBeneficiario + nossoNumero + dataVencimento + valor + documento;

            return UtilHelper.GetHashSha256(valoresAutenticacao);
        }

        public string GeraXmlBoleto(BoletoClient boletoClient)
        {
            if (boletoClient == null)
                return null;

            string _codBeneficiario = boletoClient.CodigoBeneficiario;
            long _nossoNumero = boletoClient.NossoNumero;
            DateTime _dtVencimento = boletoClient.DataVencimento;
            decimal _valor = boletoClient.Valor;

            string _autenticacao = GerarHash(boletoClient);

            HEADER header = new HEADER();
            header.VERSAO = "1.0";
            header.AUTENTICACAO = _autenticacao;
            header.USUARIO_SERVICO = "SGCBS02P";

            dynamic operacaoBoleto = null;

            if (boletoClient.Operacao == Operacao.INCLUI_BOLETO)
            {
                header.OPERACAO = "INCLUI_BOLETO";
                operacaoBoleto = new IncluiBoleto();
            }
            else if (boletoClient.Operacao == Operacao.ALTERA_BOLETO)
            {
                header.OPERACAO = "ALTERA_BOLETO";
                operacaoBoleto = new AlteraBoleto();
            }
            else if (boletoClient.Operacao == Operacao.CONSULTA_BOLETO)
            {
                header.OPERACAO = "CONSULTA_BOLETO";
                operacaoBoleto = new ConsultaBoleto();
            }

            header.SISTEMA_ORIGEM = "SIGCB";
            header.DATA_HORA = DateTime.Now.ToString("yyyyMMddHHmmss");

            operacaoBoleto.CODIGO_BENEFICIARIO = _codBeneficiario;

            var titulo = new Titulo();
            titulo.NOSSO_NUMERO = _nossoNumero; //preencher com zero caso o beneficiario não venha a controlar
            titulo.NUMERO_DOCUMENTO = boletoClient.NumeroDocumento;
            titulo.DATA_VENCIMENTO = _dtVencimento.ToString("yyyy-MM-dd");
            titulo.VALOR = Convert.ToDecimal(_valor.ToString("n2"));
            titulo.TIPO_ESPECIE = boletoClient.TipoEspecie;
            titulo.FLAG_ACEITE = boletoClient.FlagAceite;

            if (boletoClient.Operacao == Operacao.INCLUI_BOLETO)
            {
                var juros_mora = new JurosMora();

                switch (boletoClient.JurosTipo)
                {
                    case JurosTipo.ISENTO:
                        juros_mora.TIPO = "ISENTO";
                        break;
                    case JurosTipo.VALOR_POR_DIA:
                        juros_mora.TIPO = "VALOR_POR_DIA";
                        break;
                    case JurosTipo.TAXA_MENSAL:
                        juros_mora.TIPO = "TAXA_MENSAL";
                        break;
                    default:
                        break;
                }

                juros_mora.DATA = boletoClient.JurosData.ToString("yyyy-MM-dd");

                switch (boletoClient.TipoCobrancaJuros)
                {
                    case TipoCobranca.VALOR:
                        juros_mora.VALOR = boletoClient.JurosValorPercentual;
                        break;
                    case TipoCobranca.PERCENTUAL:
                        juros_mora.PERCENTUAL = boletoClient.JurosValorPercentual;
                        break;
                    default:
                        break;
                }

                titulo.JUROS_MORA = juros_mora;

                var posVencimento = new PosVencimento();

                switch (boletoClient.PosVencimentoAcao)
                {
                    case PosVencimentoAcao.DEVOLVER:
                        posVencimento.ACAO = "DEVOLVER";
                        break;
                    case PosVencimentoAcao.PROTESTAR:
                        posVencimento.ACAO = "PROTESTAR";
                        break;
                    default:
                        break;
                }

                posVencimento.NUMERO_DIAS = 0;
                titulo.POS_VENCIMENTO = posVencimento;

                titulo.CODIGO_MOEDA = "09";

                if (!string.IsNullOrEmpty(boletoClient.PagadorDocumento))
                {
                    var pagador = new Pagador();

                    if (boletoClient.PagadorDocumento.Length > 11)
                    {
                        pagador.RAZAO_SOCIAL = boletoClient.PagadorNome.CortaPalavra(40);
                        pagador.CNPJ = boletoClient.PagadorDocumento;
                    }
                    else
                    {
                        pagador.NOME = boletoClient.PagadorNome.CortaPalavra(40);
                        pagador.CPF = boletoClient.PagadorDocumento;
                    }

                    var endereco = new Endereco();
                    endereco.LOGRADOURO = boletoClient.PagadorEnderecoLogradouro.CortaPalavra(40);
                    endereco.BAIRRO = boletoClient.PagadorEnderecoBairro.CortaPalavra(15);
                    endereco.CIDADE = boletoClient.PagadorEnderecoCidade.CortaPalavra(15);
                    endereco.UF = boletoClient.PagadorEnderecoUF;
                    endereco.CEP = boletoClient.PagadorEnderecoCEP;
                    pagador.ENDERECO = endereco;
                    titulo.PAGADOR = pagador;
                }

                if (!string.IsNullOrEmpty(boletoClient.SacadorAvalistaDocumento))
                {
                    var sacadorAvalista = new SacadorAvalista();

                    if (boletoClient.SacadorAvalistaDocumento.Length > 11)
                    {
                        sacadorAvalista.CNPJ = boletoClient.SacadorAvalistaDocumento;
                        sacadorAvalista.RAZAO_SOCIAL = boletoClient.SacadorAvalistaNome.CortaPalavra(40);
                    }
                    else
                    {
                        sacadorAvalista.CPF = boletoClient.SacadorAvalistaDocumento;
                        sacadorAvalista.NOME = boletoClient.SacadorAvalistaNome.CortaPalavra(40);
                    }
                    titulo.SACADOR_AVALISTA = sacadorAvalista;
                }

                var pagamento = new Pagamento();
                pagamento.QUANTIDADE_PERMITIDA = boletoClient.PagamentoQuantidadePermitida;

                switch (boletoClient.PagamentoTipo)
                {
                    case PagamentoTipo.NAO_ACEITA_VALOR_DIVERGENTE:
                        pagamento.TIPO = "NAO_ACEITA_VALOR_DIVERGENTE";
                        break;
                    case PagamentoTipo.ACEITA_QUALQUER_VALOR:
                        pagamento.TIPO = "ACEITA_QUALQUER_VALOR";
                        break;
                    case PagamentoTipo.ACEITA_VALORES_ENTRE_MINIMO_MAXIMO:
                        pagamento.TIPO = "ACEITA_VALORES_ENTRE_MINIMO_MAXIMO";
                        break;
                    case PagamentoTipo.SOMENTE_VALOR_MINIMO:
                        pagamento.TIPO = "SOMENTE_VALOR_MINIMO";
                        break;
                    default:
                        break;
                }

                switch (boletoClient.TipoCobrancaPagamento)
                {
                    case TipoCobranca.VALOR:
                        pagamento.VALOR_MINIMO = boletoClient.PagamentoValorPercentualMinimo.ToString();
                        pagamento.VALOR_MAXIMO = boletoClient.PagamentoValorPercentualMaximo.ToString();
                        break;
                    case TipoCobranca.PERCENTUAL:
                        pagamento.PERCENTUAL_MINIMO = boletoClient.PagamentoValorPercentualMinimo.ToString();
                        pagamento.PERCENTUAL_MAXIMO = boletoClient.PagamentoValorPercentualMaximo.ToString();
                        break;
                    default:
                        break;
                }

                if (boletoClient.FichaCompensacaoMensagens != null)
                {
                    var fichaCompensacao = new FichaCompensacao();

                    fichaCompensacao.MENSAGENS = new List<string>();

                    foreach (var item in boletoClient.FichaCompensacaoMensagens)
                    {
                        fichaCompensacao.MENSAGENS.Add(item.Mensagem.CortaPalavra(40));
                    }

                    titulo.FICHA_COMPENSACAO = fichaCompensacao;
                }

                titulo.PAGAMENTO = pagamento;
            }

            if (boletoClient.Operacao != Operacao.CONSULTA_BOLETO)
            {
                operacaoBoleto.TITULO = titulo;
            }

            DADOS dados = new DADOS();

            if (boletoClient.Operacao == Operacao.INCLUI_BOLETO)
            {
                dados.INCLUI_BOLETO = operacaoBoleto;
            }
            else if (boletoClient.Operacao == Operacao.ALTERA_BOLETO)
            {
                dados.ALTERA_BOLETO = operacaoBoleto;
            }
            else if (boletoClient.Operacao == Operacao.CONSULTA_BOLETO)
            {
                dados.CONSULTA_BOLETO = operacaoBoleto;
                dados.CONSULTA_BOLETO.NOSSO_NUMERO = _nossoNumero;
            }

            SERVICO_ENTRADA boleto = new SERVICO_ENTRADA();
            boleto.HEADER = header;
            boleto.DADOS = dados;

            SoapEnvelope soap = new SoapEnvelope();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            if (boletoClient.Operacao == Operacao.CONSULTA_BOLETO)
            {
                soap.body = new ResponseBody<SERVICO_ENTRADA>();
                soap.body.SERVICO_ENTRADA_Consulta = boleto;

                ns.Add("ext", "http://caixa.gov.br/sibar/consulta_cobranca_bancaria/boleto");
            }
            else
            {
                soap.body = new ResponseBody<SERVICO_ENTRADA>();
                soap.body.SERVICO_ENTRADA = boleto;

                ns.Add("ext", "http://caixa.gov.br/sibar/manutencao_cobranca_bancaria/boleto/externo");
            }

            ns.Add("sib", "http://caixa.gov.br/sibar");

            return XmlHelper.Serialize(soap, ns);
        }

        static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void settingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}