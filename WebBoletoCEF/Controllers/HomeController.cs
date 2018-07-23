using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using WebServiceCaixaEconomicaFederal;
using WebServiceCaixaEconomicaFederal.ViewModel;

namespace WebBoletoCEF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GerarBoleto()
        {
            BoletoClient client = new BoletoClient();
            BoletoCEF boletoCef = new BoletoCEF();

            long _nossoNumero = 14000000000000001;
            string _codigoBeneficiario = "123456";
            string _beneficiarioDocumento = "11111111111111";

            client.Operacao = Operacao.CONSULTA_BOLETO;
            client.CodigoBeneficiario = _codigoBeneficiario;
            client.NossoNumero = _nossoNumero;
            client.BeneficiarioDocumento = _beneficiarioDocumento;

            var consulta = boletoCef.ManutencaoBoleto(client);

            if (consulta == null)
            {
                client = new BoletoClient();
                client.Operacao = Operacao.INCLUI_BOLETO;
                client.CodigoBeneficiario = _codigoBeneficiario;
                client.BeneficiarioDocumento = _beneficiarioDocumento;
                client.NossoNumero = _nossoNumero;
                client.NumeroDocumento = "1";
                client.DataVencimento = DateTime.Now.AddDays(3);
                client.Valor = 10m;
                client.TipoEspecie = "02";
                client.FlagAceite = "S";
                client.JurosTipo = JurosTipo.ISENTO;
                client.PosVencimentoAcao = PosVencimentoAcao.DEVOLVER;
                client.PosVencimentoNumeroDias = 0;
                client.CodigoMoeda = 09;
                client.PagadorDocumento = "22222222222";
                client.PagadorNome = "Pagador teste - CPF:22222222222";
                client.PagadorEnderecoLogradouro = "Rua Barão de Duprat";
                client.PagadorEnderecoBairro = "Centro";
                client.PagadorEnderecoCidade = "São Paulo";
                client.PagadorEnderecoUF = "SP";
                client.PagadorEnderecoCEP = "01023001";
                client.SacadorAvalistaDocumento = "";
                client.SacadorAvalistaNome = "";
                client.PagamentoQuantidadePermitida = 1;
                client.PagamentoTipo = PagamentoTipo.NAO_ACEITA_VALOR_DIVERGENTE;
                client.TipoCobrancaPagamento = TipoCobranca.VALOR;
                client.PagamentoValorPercentualMinimo = 0;
                client.PagamentoValorPercentualMaximo = 0;

                client.FichaCompensacaoMensagens = new List<Mensagens>()
                        {
                            new Mensagens(){ Mensagem =  "Não aceitar pagamento após vencimento.".ToUpper()},
                            new Mensagens(){ Mensagem = "Ref.: Teste de emissão de boleto da CEF." }
                        };

                var retorno = boletoCef.ManutencaoBoleto(client);

                if (retorno != null)
                {
                    string nameDownload = client.CodigoBeneficiario + "_" + client.NossoNumero;
                    VisualizaPDF(retorno.URL, nameDownload);
                }
            }
            else
            {
                client = new BoletoClient();
                client.Operacao = Operacao.ALTERA_BOLETO;
                client.CodigoBeneficiario = _codigoBeneficiario;
                client.BeneficiarioDocumento = _beneficiarioDocumento;
                client.DataVencimento = DateTime.Now.AddDays(6);
                client.Valor = 10m;
                
                client.NossoNumero = _nossoNumero;

                boletoCef = new BoletoCEF();
                var retorno = boletoCef.ManutencaoBoleto(client);

                if (retorno != null)
                {
                    string nameDownload = client.CodigoBeneficiario + "_" + client.NossoNumero;
                    VisualizaPDF(retorno.URL, nameDownload);
                }
            }

            return null;
        }

        private void VisualizaPDF(string url, string nameDownload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

            Stream stream = resp.GetResponseStream();
            MemoryStream workStream = new MemoryStream();
            CopyStream(stream, workStream);

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", string.Format("inline;filename=\"{0}\"", "Boleto" + nameDownload + ".pdf"));
            Response.BinaryWrite(byteInfo);
            Response.End();
            stream.Close();
            workStream.Dispose();
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}