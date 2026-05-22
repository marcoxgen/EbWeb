using EbWeb.Models.AlimentazioneBudget.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AlimentazioneBudget.InputModels;

public class PubblicazioneBudgetCreateInputModel
{
    [Required(ErrorMessage = "Il tipo pubblicazione è obbligatorio")]
    [Display(Name = "Tipo Pubblicazione")]
    public string TipoCod { get; set; }

    [Required(ErrorMessage = "La data di riferimento è obbligatoria")]
    [Display(Name = "Data di Riferimento")]
    public DateOnly DataRiferimento { get; set; } 

    public List<TipoPubblicazioneLookupViewModel> TipiLookup { get; set; } = new();

    public List<CalendarioDinamicoLookupViewModel> CalendarioLookup { get; set; } = new();
}
