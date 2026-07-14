using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AbilitazioniMifid.InputModels;

public class AbilitazioneIvassDeleteInputModel
{
    [Required]
    public int Matricola { get; set; }
}