using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain.Tests
{
    public class AlunoDomainTests
    {
        [Fact]
        public void CriarAluno_Valido_NaoDeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");

            var aluno = Aluno.Criar("12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "senha123", null,
                                    logradouro, "100", null);

            Assert.NotNull(aluno);
        }

        [Fact]
        public void CriarAluno_CpfVazio_DeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");

            Assert.Throws<DomainException>(() =>
                Aluno.Criar("", "João", new DateOnly(2000, 1, 1),
                            "joao@email.com", "48999999999", "senha123", null,
                            logradouro, "100", null)
            );
        }

        [Fact]
        public void CriarAluno_LogradouroNulo_DeveLancarExcecao()
        {
            Assert.Throws<DomainException>(() =>
                Aluno.Criar("12345678901", "João", new DateOnly(2000, 1, 1),
                            "joao@email.com", "48999999999", "senha123", null,
                            null!, "100", null)
            );
        }
    }
}
