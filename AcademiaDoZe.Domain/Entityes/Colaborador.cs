using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;
using AcademiaDoZe.Domain.Service;

namespace AcademiaDoZe.Domain
{
    public sealed class Colaborador : Pessoa
    {
        public DateOnly DataAdmissao { get; set; }
        public EColaboradorTipo Tipo { get; set; }
        public EColaboradorVinculo Vinculo { get; set; }

        private Colaborador(int id, DateOnly dataAdmissao, EColaboradorTipo tipoColaborador, EColaboradorVinculo vinculo,
            string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, Arquivo? foto, Logradouro logradouro,
            string numero, string complemento) : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {

            DataAdmissao = dataAdmissao;
            Tipo = tipoColaborador;
            Vinculo = vinculo;
        }

        public static Colaborador Criar(int id, DateOnly dataAdmissao, EColaboradorTipo tipo, EColaboradorVinculo vinculo,
            string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, Arquivo? foto, Logradouro endereco,
            string numero, string complemento)
        {

            if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("NOME_OBRIGATORIO");
            nome = NormalizadoService.LimparEspacos(nome);
            if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");
            cpf = NormalizadoService.LimparEDigitos(cpf);
            if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");
            if (dataNascimento == default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIO");
            if (dataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-12))) throw new DomainException("DATA_NASCIMENTO_MINIMA_INVALIDA");
            if (string.IsNullOrWhiteSpace(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
            telefone = NormalizadoService.LimparEDigitos(telefone);
            if (telefone.Length != 11) throw new DomainException("TELEFONE_DIGITOS");
            email = NormalizadoService.LimparEspacos(email);
            if (NormalizadoService.ValidarFormatoEmail(email)) throw new DomainException("EMAIL_FORMATO");
            if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("SENHA_OBRIGATORIO");
            senha = NormalizadoService.LimparEspacos(senha);
            if (NormalizadoService.ValidarFormatoSenha(senha)) throw new DomainException("SENHA_FORMATO");
            if (foto == null) throw new DomainException("FOTO_OBRIGATORIO");
            if (endereco == null) throw new DomainException("LOGRADOURO_OBRIGATORIO");
            if (string.IsNullOrWhiteSpace(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
            numero = NormalizadoService.LimparEspacos(numero);
            complemento = NormalizadoService.LimparEspacos(complemento);
            if (dataAdmissao == default) throw new DomainException("DATA_ADMISSAO_OBRIGATORIO");
            if (dataAdmissao > DateOnly.FromDateTime(DateTime.Today)) throw new DomainException("DATA_ADMISSAO_MAIOR_ATUAL");
            if (!Enum.IsDefined(tipo)) throw new DomainException("TIPO_COLABORADOR_INVALIDO");
            if (!Enum.IsDefined(vinculo)) throw new DomainException("VINCULO_COLABORADOR_INVALIDO");
            if (tipo == EColaboradorTipo.Administrador && vinculo != EColaboradorVinculo.CLT) throw new DomainException("ADMINISTRADOR_CLT_INVALIDO");

            return new Colaborador(id, dataAdmissao, tipo, vinculo, cpf, nome, dataNascimento, email,
                telefone, senha, foto, endereco, numero, complemento);
        }
    }
}
