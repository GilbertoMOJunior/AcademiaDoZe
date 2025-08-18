//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain.Tests
{
    public class LogradouroDomainTests
    {
        [Fact]
        public void CriarLogradouro_Valido_NaoDeveLancarExcecao()
        {
            var logradouro = Logradouro.Criar("Casa", "12345678", "Brasil", "SP", "SP", "Centro");
            Assert.NotNull(logradouro); // validando criação, não deve lançar exceção e não deve ser nulo
        }
        
        [Fact]
        public void CriarLogradouro_Valido_DeveLancarExcecao()
        {
            Assert.Throws<DomainException>(() => Logradouro.Criar("Casa", "1234567", "Brasil", "SP", "SP", "Centro"));      
        }

        [Fact]
        public void CriarLogradouro_Valido_VerificarNormalizado()
        {
            var logradouro = Logradouro.Criar("Casa  ", "1234./567-8", "  Brasil  ", "S P", " SP ", "  Centro");
            Assert.Equal("12345678", logradouro.Cep); // validando normalização
            Assert.Equal("Casa", logradouro.Nome);
            Assert.Equal("Centro", logradouro.Bairro);
            Assert.Equal("SP", logradouro.Cidade);
            Assert.Equal("SP", logradouro.Estado);
            Assert.Equal("Brasil", logradouro.Pais);
        }

        [Fact]
        public void CriarLogradouro_Invalido_VerificarMessageExcecao()
        {
            var exception = Assert.Throws<DomainException>(() => Logradouro.Criar("", "12345678", "Brasil", "SP", "SP", "Centro"));
            Assert.Equal("Nome do logradouro não pode ser vazio.", exception.Message); // validando a mensagem de exceção
        }
    }
}
