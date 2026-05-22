using EbWeb.Models.AbilitazioniMifid.ViewModels;

namespace EbWeb.Models.AbilitazioniMifid.InputModels;

public class AbilitazioneMifidCreateInputModel
{
    public int Matricola { get; set; }
    public List<AnagDipendentiLookupViewModel> AnagDipendentiLookup { get; set; } = new();
}