//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain;
namespace AcademiaDoZe.Application.Mappings;

public static class ColaboradorMappings
{
    public static ColaboradorDTO ToDto(this Colaborador colaborador)
    {
        return new ColaboradorDTO
        {
            Id = colaborador.Id,
            Nome = colaborador.Nome,
            Cpf = colaborador.Cpf,
            DataNascimento = colaborador.DataNascimento,
            Telefone = colaborador.Telefone,
            Email = colaborador.Email,
            Endereco = colaborador.Endereco.ToDto(),
            Numero = colaborador.Numero,
            Complemento = colaborador.Complemento,
            Senha = null, // A senha não deve ser exposta no DTO
            Foto = colaborador.Foto != null ? new ArquivoDTO { Conteudo = colaborador.Foto.Conteudo } : null, // Mapeia a foto para DTO
            DataAdmissao = colaborador.DataAdmissao,
            Tipo = colaborador.Tipo.ToApp(),
            Vinculo = colaborador.Vinculo.ToApp()
        };
    }
    public static Colaborador ToEntity(this ColaboradorDTO colaboradorDto)
    {
        return Colaborador.Criar(
        id: colaboradorDto.Id,
        nome: colaboradorDto.Nome,
        cpf:colaboradorDto.Cpf,
        dataNascimento:colaboradorDto.DataNascimento,
        telefone:colaboradorDto.Telefone,
        email:colaboradorDto.Email!,
        endereco:colaboradorDto.Endereco.ToEntity(), // Mapeia o logradouro do DTO para a entidade
        numero:colaboradorDto.Numero,
        complemento:colaboradorDto.Complemento!,
        senha:colaboradorDto.Senha!,
        foto:(colaboradorDto.Foto?.Conteudo != null) ? Arquivo.Criar(colaboradorDto.Foto.Conteudo, ".jpg") : null!, // Mapeia a foto do DTO para a entidade
        dataAdmissao:colaboradorDto.DataAdmissao,
        tipo:colaboradorDto.Tipo.ToDomain(),
        vinculo:colaboradorDto.Vinculo.ToDomain()
        );
    }
    public static Colaborador UpdateFromDto(this Colaborador colaborador, ColaboradorDTO colaboradorDto)
    {
        return Colaborador.Criar(
        id:colaborador.Id,
        nome:colaboradorDto.Nome ?? colaborador.Nome,
        cpf:colaborador.Cpf, // CPF não pode ser alterado
        dataNascimento:colaboradorDto.DataNascimento != default ? colaboradorDto.DataNascimento : colaborador.DataNascimento,
        telefone:colaboradorDto.Telefone ?? colaborador.Telefone,
        email:colaboradorDto.Email ?? colaborador.Email,
        endereco:colaboradorDto.Endereco.ToEntity() ?? colaborador.Endereco,
        numero:colaboradorDto.Numero ?? colaborador.Numero,
        complemento:colaboradorDto.Complemento ?? colaborador.Complemento,
        senha:colaboradorDto.Senha ?? colaborador.Senha,
        foto:(colaboradorDto.Foto?.Conteudo != null) ? Arquivo.Criar(colaboradorDto.Foto.Conteudo) : colaborador.Foto, // Atualiza a foto se fornecida
        dataAdmissao:colaboradorDto.DataAdmissao != default ? colaboradorDto.DataAdmissao : colaborador.DataAdmissao,
        tipo:colaboradorDto.Tipo != default ? colaboradorDto.Tipo.ToDomain() : colaborador.Tipo,
        vinculo:colaboradorDto.Vinculo != default ? colaboradorDto.Vinculo.ToDomain() : colaborador.Vinculo
        );
    }
}