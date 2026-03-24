using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.InputModels;

public class AbilitazioneMifidDeleteInputModel
{
    [Required]
    public int Matricola { get; set; }
}