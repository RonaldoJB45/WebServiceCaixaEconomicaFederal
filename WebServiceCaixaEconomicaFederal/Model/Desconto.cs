using System;

namespace WebServiceCaixaEconomicaFederal.Model
{
    public class Desconto
    {
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
    }
}