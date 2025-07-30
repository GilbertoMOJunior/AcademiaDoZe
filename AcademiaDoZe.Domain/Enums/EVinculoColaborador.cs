using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EVinculoColaborador
    {
        [Display(Name = "CLT")]
        CLT = 1,

        [Display(Name = "Estágiario")]
        Estagio = 2
    }
}
