//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain.Tests
{
    public class MatriculaDomainTests
    {
        [Fact]
        public void CriarMatricula_Valida_NaoDeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");
            var foto = Arquivo.Criar(new byte[1], ".jpg");
            var aluno = Aluno.Criar(1, "12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "Senha123", foto,
                                    logradouro, "100", null);

            var matricula = Matricula.Criar(aluno, EMatriculaPlano.Mensal,
                                           DateOnly.FromDateTime(DateTime.Now),
                                           DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                                           "Musculação", EMatriculaRestricoes.Nenhuma, "", null);

            Assert.NotNull(matricula);
        }

        [Fact]
        public void CriarMatricula_AlunoMenor12_DeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");
           // var aluno = Aluno.Criar(1, "12345678901", "Pedro", DateOnly.FromDateTime(DateTime.Now.AddYears(-10)),
            //                        null, "48999999999", "senha123", null,
              //                      logradouro, "100", null);

            Assert.Throws<DomainException>(() =>
                Aluno.Criar(1, "12345678901", "Pedro", DateOnly.FromDateTime(DateTime.Now.AddYears(-10)),
                                    null, "48999999999", "senha123", null,
                                    logradouro, "100", null)
            );
        }

        [Fact]
        public void CriarMatricula_DataInicioMaiorQueDataFim_DeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Rua A", "12345678", "Brasil", "SP", "SP", "Centro");
            var foto = Arquivo.Criar(new byte[1], ".jpg");
            var aluno = Aluno.Criar(1, "12345678901", "João", new DateOnly(2000, 1, 1),
                                    "joao@email.com", "48999999999", "Senha123", foto,
                                    logradouro, "100", null);

            Assert.Throws<DomainException>(() =>
                Matricula.Criar(aluno, EMatriculaPlano.Mensal,
                                DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                                DateOnly.FromDateTime(DateTime.Now),
                                "Musculação", EMatriculaRestricoes.Nenhuma, "", null)
            );
        }
    }
}
