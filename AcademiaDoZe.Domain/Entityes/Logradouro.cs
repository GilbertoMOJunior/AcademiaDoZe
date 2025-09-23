using AcademiaDoZe.Domain.Exeption;
using AcademiaDoZe.Domain.Service;

namespace AcademiaDoZe.Domain
{
    public sealed class Logradouro : Entity
    {
        public string Cep { get; set; }
        public string Nome { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }

        private Logradouro(int id, string nomeLogradouro, string cEP, string pais, string estado, string cidade, string bairro) : base(id)
        {
            Nome = nomeLogradouro;
            Cep = cEP;
            Pais = pais;
            Cidade = cidade;
            Bairro = bairro;
            Estado = estado;
        }

        public static Logradouro Criar(int id, string cep, string nome, string pais, string estado, string cidade, string bairro)
        {
            cep = NormalizadoService.LimparEDigitos(cep);
            nome = NormalizadoService.LimparEspacos(nome);
            pais = NormalizadoService.LimparEspacos(pais);
            estado = NormalizadoService.LimparTodosEspacos(estado);
            cidade = NormalizadoService.LimparEspacos(cidade);
            bairro = NormalizadoService.LimparEspacos(bairro);

            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome do logradouro não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(cep))
                throw new DomainException("CEP não pode ser vazio.");
            if (cep.Length != 8)
                throw new DomainException("CEP deve conter 8 dígitos.");
            if (string.IsNullOrWhiteSpace(pais))
                throw new DomainException("País não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(estado))
                throw new DomainException("Estado não pode ser vazio.");
            if (estado.Length > 2)
                throw new DomainException("Estado não pode ter mais de 2 digitos.");
            if (string.IsNullOrWhiteSpace(cidade))
                throw new DomainException("Cidade não pode ser vazia.");
            if (string.IsNullOrWhiteSpace(bairro))
                throw new DomainException("Bairro não pode ser vazio.");

            return new Logradouro(id, nome, cep, pais, estado, cidade, bairro);
        }
    }
}
