// Gilberto Mota de Oliveira Junior
using System.ComponentModel.DataAnnotations;
namespace AcademiaDoZe.Application.Enums;

public enum EAppPessoaTipo
{
    [Display(Name = "Colaborador")]
    Colaborador = 0,
    [Display(Name = "Aluno")]
    Aluno = 1
}