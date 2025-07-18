namespace AcademiaDoZe.Domain
{
    public sealed class Logradouro
    {
        public Logradouro(string nomeLogradouro, string cEP, string pais, string cidade, string bairro)
        {
            NomeLogradouro = nomeLogradouro;
            CEP = cEP;
            Pais = pais;
            Cidade = cidade;
            Bairro = bairro;
        }

        public string NomeLogradouro { get; set; }
        public string CEP { get; set; }
        public string Pais { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
    }
}
