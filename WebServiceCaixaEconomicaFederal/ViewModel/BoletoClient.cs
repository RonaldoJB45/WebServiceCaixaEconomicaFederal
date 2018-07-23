using System;
using System.Collections.Generic;

namespace WebServiceCaixaEconomicaFederal.ViewModel
{
    public class BoletoClient
    {
        public Operacao Operacao { get; set; }
        public string CodigoBeneficiario { get; set; }
        public string BeneficiarioDocumento { get; set; }
        public long NossoNumero { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public string TipoEspecie { get; set; }
        public string FlagAceite { get; set; }
        public JurosTipo JurosTipo { get; set; }
        public DateTime JurosData { get; set; }
        public TipoCobranca TipoCobrancaJuros { get; set; }
        public decimal JurosValorPercentual { get; set; }
        public PosVencimentoAcao PosVencimentoAcao { get; set; }
        public int PosVencimentoNumeroDias { get; set; }
        public short CodigoMoeda { get; set; }
        public string PagadorDocumento { get; set; }
        public string PagadorNome { get; set; }
        public string PagadorEnderecoLogradouro { get; set; }
        public string PagadorEnderecoBairro { get; set; }
        public string PagadorEnderecoCidade { get; set; }
        public string PagadorEnderecoUF { get; set; }
        public string PagadorEnderecoCEP { get; set; }
        public string SacadorAvalistaDocumento { get; set; }
        public string SacadorAvalistaNome { get; set; }
        public short PagamentoQuantidadePermitida { get; set; }
        public PagamentoTipo PagamentoTipo { get; set; }
        public TipoCobranca TipoCobrancaPagamento { get; set; }
        public decimal PagamentoValorPercentualMinimo { get; set; }
        public decimal PagamentoValorPercentualMaximo { get; set; }
        public List<Mensagens> FichaCompensacaoMensagens { get; set; }
    }

    public class Mensagens
    {
        public string Mensagem { get; set; }
    }

    public enum Operacao
    {
        INCLUI_BOLETO, ALTERA_BOLETO, CONSULTA_BOLETO
    }

    public enum JurosTipo
    {
        ISENTO, VALOR_POR_DIA, TAXA_MENSAL
    }
    public enum TipoCobranca
    {
        VALOR, PERCENTUAL
    }
    public enum PosVencimentoAcao
    {
        DEVOLVER, PROTESTAR
    }
    public enum PagamentoTipo
    {
        NAO_ACEITA_VALOR_DIVERGENTE, ACEITA_QUALQUER_VALOR, ACEITA_VALORES_ENTRE_MINIMO_MAXIMO, SOMENTE_VALOR_MINIMO
    }
}