//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain;

namespace AcademiaDoZe.Application.Mappings;

public static class LogradouroMappings
{
    public static LogradouroDTO ToDto(this Logradouro logradouro)
    {
        return new LogradouroDTO
        {
            Id = logradouro.Id,
            Cep = logradouro.Cep,
            Nome = logradouro.Nome,
            Bairro = logradouro.Bairro,
            Cidade = logradouro.Cidade,
            Estado = logradouro.Estado,
            Pais = logradouro.Pais
        };
    }
    public static Logradouro ToEntity(this LogradouroDTO logradouroDto)
    {
        return Logradouro.Criar(
        logradouroDto.Id,
        logradouroDto.Cep,
        logradouroDto.Nome,
        logradouroDto.Pais,
        logradouroDto.Estado,
        logradouroDto.Cidade,
        logradouroDto.Bairro);
    }
    public static Logradouro UpdateFromDto(this Logradouro logradouro, LogradouroDTO logradouroDto)
    {
        // Cria uma nova instância do Logradouro com os valores atualizados

        return Logradouro.Criar(
        logradouroDto.Id,
        logradouro.Cep, // Mantém o CEP original
        logradouroDto.Nome ?? logradouro.Nome,
        logradouroDto.Pais ?? logradouro.Pais,
        logradouroDto.Estado ?? logradouro.Estado,
        logradouroDto.Cidade ?? logradouro.Cidade,
        logradouroDto.Bairro ?? logradouro.Bairro);
    }
}