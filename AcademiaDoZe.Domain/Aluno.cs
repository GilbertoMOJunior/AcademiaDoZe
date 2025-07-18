namespace AcademiaDoZe.Domain
{
    public sealed class Aluno : Pessoa
    {
        public Aluno(string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, string? foto, Logradouro logradouro, string numero, string? complemento)
            : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {

        }

        public string GetTempoPermanencia(DateTime Inicio, DateTime FIm)
        {
            throw new NotImplementedException("Método GetTempoPermanencia não implementado.");
        }

        public string GetTempoContrato()
        {
            throw new NotImplementedException("Método GetTempoContrato não implementado.");
            //contrato = Select Contrato where AlunoId = this.Id
            //return $"Data vencimento:{contrato.DataVencimento};
        }

        public void TrocarSenha(string senhaAtual, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(senhaAtual))
                throw new ArgumentException("Senha atual não pode ser vazia.", nameof(senhaAtual));
            if (string.IsNullOrWhiteSpace(novaSenha))
                throw new ArgumentException("Nova senha não pode ser vazia.", nameof(novaSenha));
            if (senhaAtual != Senha)
                throw new InvalidOperationException("Senha atual está incorreta.");
            Senha = novaSenha;
        }
    }
}