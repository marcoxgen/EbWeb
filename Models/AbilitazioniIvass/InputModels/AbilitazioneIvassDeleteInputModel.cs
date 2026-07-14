using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AbilitazioniIvass.InputModels;

public class AbilitazioneIvassDeleteInputModel
{
    [Required]
    public int Matricola { get; set; }
}