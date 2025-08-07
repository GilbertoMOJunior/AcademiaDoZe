using System.Runtime.ConstrainedExecution;
using AcademiaDoZe.Domain.Exeption;
using AcademiaDoZe.Domain.Service;

namespace AcademiaDoZe.Domain
{
    public sealed class Logradouro : Entity
    {
        private Logradouro(string nomeLogradouro, string cEP, string pais, string estado, string cidade, string bairro)
        {
            NomeLogradouro = nomeLogradouro;
            CEP = cEP;
            Pais = pais;
            Cidade = cidade;
            Bairro = bairro;
            Estado = estado;
        }

        public static Logradouro Criar(string nomeLogradouro, string cEP, string pais, string estado, string cidade, string bairro)
        {
            cEP = TextoNormalizadoService.LimparEDigitos(cEP);
            nomeLogradouro = TextoNormalizadoService.LimparEspacos(nomeLogradouro);
            pais = TextoNormalizadoService.LimparEspacos(pais);
            estado = TextoNormalizadoService.LimparTodosEspacos(estado);
            cidade = TextoNormalizadoService.LimparEspacos(cidade);
            bairro = TextoNormalizadoService.LimparEspacos(bairro);

            if (string.IsNullOrWhiteSpace(nomeLogradouro))
                throw new DomainException("Nome do logradouro não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(cEP))
                throw new DomainException("CEP não pode ser vazio.");
            if(cEP.Length != 8)
                throw new DomainException("CEP deve conter 8 dígitos.");
            if (string.IsNullOrWhiteSpace(pais))
                throw new DomainException("País não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(estado))
                throw new DomainException("Estado não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(cidade))
                throw new DomainException("Cidade não pode ser vazia.");
            if (string.IsNullOrWhiteSpace(bairro))
                throw new DomainException("Bairro não pode ser vazio.");

            return new Logradouro(nomeLogradouro, cEP, pais, estado, cidade, bairro);
        }

        public string NomeLogradouro { get; set; }
        public string CEP { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
