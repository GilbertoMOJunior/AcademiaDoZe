//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain;
namespace AcademiaDoZe.Application.Mappings;

public static class AlunoMappings
{
    public static AlunoDTO ToDto(this Aluno aluno)
    {
        return new AlunoDTO
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Cpf = aluno.Cpf,
            DataNascimento = aluno.DataNascimento,
            Telefone = aluno.Telefone,
            Email = aluno.Email,
            Endereco = aluno.Endereco.ToDto(), // Mapeia o logradouro para DTO
            Numero = aluno.Numero,
            Complemento = aluno.Complemento,
            Senha = null, // a senha não deve ser exposta no DTO
            Foto = aluno.Foto != null ? new ArquivoDTO { Conteudo = aluno.Foto.Conteudo } : null // Mapeia a foto para DTO
        };
    }
    public static Aluno ToEntity(this AlunoDTO alunoDto)
    {
        return Aluno.Criar(
        alunoDto.Id, alunoDto.Nome, alunoDto.Cpf, alunoDto.DataNascimento, alunoDto.Telefone, alunoDto.Email!,
        endereco:alunoDto.Endereco.ToEntity(), // Mapeia o logradouro do DTO para a entidade
        numero:alunoDto.Numero, complemento:alunoDto.Complemento!, senha:alunoDto.Senha!, foto:(alunoDto.Foto?.Conteudo != null) ? Arquivo.Criar(alunoDto.Foto.Conteudo, ".jpg") : null!
        );
    }
    /*
    * A camada de aplicação não expõe a senha do Aluno na DTO, ao tentar usar essas entidades, por exemplo, na matricula, pode ocorrer erro de validação/normalização do domínio, quando a DTO for mapeada para a entidade.
    * Ou seja, ao tentar salvar uma nova matricula, a DTO do Aluno vai ser mapeada para a entidade Aluno para poder ser enviada a camada de Infraestrutura, porém, como na DTO a senha do Aluno foi definida como null, ao
    realizar o mapeamento, a validação de domínio da entidade falha, pois a senha é uma campo obrigatório.
    * A prática mais robusta para resolver isso, é criar um DTO ou Mapeamento específico para o caso de uso.
    * O mapeamento ToEntityMatricula() será utilizado exclusivamente na Matricula, e mascara a senha, passando desta forma pela validação.
    */
    public static Aluno ToEntityMatricula(this AlunoDTO alunoDto)
    {
        return Aluno.Criar(
        id:alunoDto.Id, 
        nome:alunoDto.Nome,
        cpf:alunoDto.Cpf,
        dataNascimento:alunoDto.DataNascimento,
        telefone:alunoDto.Telefone,
        email:alunoDto.Email!,
        endereco:alunoDto.Endereco.ToEntity(),
        numero:alunoDto.Numero, 
        complemento:alunoDto.Complemento!,
        senha:"senhaFalsaSomenteParaAtenderDominio@123",
        foto:(alunoDto.Foto?.Conteudo != null) ? Arquivo.Criar(alunoDto.Foto.Conteudo, ".jpg") : null!
        );
    }
    public static Aluno UpdateFromDto(this Aluno aluno, AlunoDTO alunoDto)
    {
        return Aluno.Criar(
        id:alunoDto.Id,
        nome: alunoDto.Nome ?? aluno.Nome,
        cpf:aluno.Cpf, // CPF não pode ser alterado
        dataNascimento: alunoDto.DataNascimento != default ? alunoDto.DataNascimento : aluno.DataNascimento,
        telefone:alunoDto.Telefone ?? aluno.Telefone,
        email:alunoDto.Email ?? aluno.Email,
        endereco:alunoDto.Endereco.ToEntity() ?? aluno.Endereco, 
        numero:alunoDto.Numero ?? aluno.Numero, 
        complemento:alunoDto.Complemento ?? aluno.Complemento,
        senha:alunoDto.Senha ?? aluno.Senha, 
        foto:(alunoDto.Foto?.Conteudo != null) ? Arquivo.Criar(alunoDto.Foto.Conteudo, ".jpg") : aluno.Foto // Atualiza a foto se fornecida
        );
    }
}