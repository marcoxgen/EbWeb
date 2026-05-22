using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AbilitazioniMifid.InputModels;

public class AbilitazioneMifidDeleteInputModel
{
    [Required]
    public int Matricola { get; set; }
}