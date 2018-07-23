namespace WebServiceCaixaEconomicaFederal.Model
{
    public class Titulo
    {
        public long NOSSO_NUMERO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string DATA_VENCIMENTO { get; set; }
        public decimal VALOR { get; set; }
        public string TIPO_ESPECIE { get; set; }
        public string FLAG_ACEITE { get; set; }
        public JurosMora JUROS_MORA { get; set; }
        public PosVencimento POS_VENCIMENTO { get; set; }
        public string CODIGO_MOEDA { get; set; }
        public Pagador PAGADOR { get; set; }
        public SacadorAvalista SACADOR_AVALISTA { get; set; }
        public Multa MULTA { get; set; }
        public Descontos DESCONTOS { get; set; }
        public string VALOR_IOF { get; set; }
        public string IDENTIFICACAO_EMPRESA { get; set; }
        public FichaCompensacao FICHA_COMPENSACAO { get; set; }
        public ReciboPagador RECIBO_PAGADOR { get; set; }
        public Pagamento PAGAMENTO { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string LINHA_DIGITAVEL { get; set; }
        public string URL { get; set; }
    }
}