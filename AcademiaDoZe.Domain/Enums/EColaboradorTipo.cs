using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    public enum EColaboradorTipo
    {
        [Display(Name = "Administrador")]
        Administrador = 1,

        [Display(Name = "Atendente")]
        Atendente = 2,

        [Display(Name = "Instrutor")]
        Instrutor = 3
    }
}
