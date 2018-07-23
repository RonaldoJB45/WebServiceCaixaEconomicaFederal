namespace WebServiceCaixaEconomicaFederal.Model
{
    public class Pagador
    {
        public string CPF { get; set; }
        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrEmpty(this.CPF);
        }

        public string CNPJ { get; set; }
        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrEmpty(this.CNPJ);
        }

        public string NOME { get; set; }
        public bool ShouldSerializeNOME()
        {
            return !string.IsNullOrEmpty(this.NOME);
        }

        public string RAZAO_SOCIAL { get; set; }
        public bool ShouldSerializeRAZAO_SOCIAL()
        {
            return !string.IsNullOrEmpty(this.RAZAO_SOCIAL);
        }

        public Endereco ENDERECO { get; set; }
    }
}