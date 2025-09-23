//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain.Tests
{
    public class AlunoDomainTests
    {
        [Fact]
        public void CriarAluno_Valido_NaoDeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar(1,"Rua A", "12345678", "Brasil", "SP", "SP", "Centro");

            var foto = Arquivo.Criar(new byte[1], ".jpg");

            var aluno = Aluno.Criar(1, "12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "Senha123", foto,
                                    logradouro, "100", null);

            Assert.NotNull(aluno);
        }

        [Fact]
        public void CriarAluno_CpfVazio_DeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar(1, "Rua A", "12345678", "Brasil", "SP", "SP", "Centro");

            Assert.Throws<DomainException>(() =>
                Aluno.Criar(1, "", "João", new DateOnly(2000, 1, 1),
                            "joao@email.com", "48999999999", "senha123", null,
                            logradouro, "100", null)
            );
        }

        [Fact]
        public void CriarAluno_LogradouroNulo_DeveLancarExcecao()
        {
            Assert.Throws<DomainException>(() =>
                Aluno.Criar(1, "12345678901", "João", new DateOnly(2000, 1, 1),
                            "joao@email.com", "48999999999", "senha123", null,
                            null!, "100", null)
            );
        }
    }
}
