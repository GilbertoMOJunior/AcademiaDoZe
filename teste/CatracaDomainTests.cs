//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain.Tests
{
    public class CatracaDomainTests
    {
        [Fact]
        public void CriarCatraca_Valida_NaoDeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");
            var aluno = Aluno.Criar("12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "senha123", null,
                                    logradouro, "100", null);

            var catraca = Acesso.Criar(aluno, DateTime.Now,Enums.ETipoPessoa.Aluno);

            Assert.NotNull(catraca);
        }

        [Fact]
        public void CriarCatraca_PessoaNula_DeveLancarExcecao()
        {
            Assert.Throws<DomainException>(() =>
                Acesso.Criar(null!, DateTime.Now, Enums.ETipoPessoa.Aluno)
            );
        }

        [Fact]
        public void CriarCatraca_DataHoraInvalida_DeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");
            var aluno = Aluno.Criar("12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "senha123", null,
                                    logradouro, "100", null);

            Assert.Throws<DomainException>(() =>
                Acesso.Criar(aluno, default, Enums.ETipoPessoa.Aluno)
            );
        }
    }
}
