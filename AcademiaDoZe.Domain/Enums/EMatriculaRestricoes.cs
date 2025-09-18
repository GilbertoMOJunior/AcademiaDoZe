using System.ComponentModel.DataAnnotations;

namespace AcademiaDoZe.Domain.Enums
{
    [Flags]
    public enum EMatriculaRestricoes
    {
        [Display(Name = "Nenhuma restrição")]
        Nenhuma = 0,

        [Display(Name = "Cardíaca")]
        Cardiaca = 1,

        [Display(Name = "Respiratória")]
        Respiratoria = 2,

        [Display(Name = "Lesão Articular")]
        LesaoArticular = 4,

        [Display(Name = "Outros")]
        Outros = 8
    }
}

