using EbWeb.Models.ViewModels;

namespace EbWeb.Models.InputModels;

public class AbilitazioneMifidCreateInputModel
{
    public int Matricola { get; set; }
    public List<AnagDipendentiLookupViewModel> AnagDipendentiLookup { get; set; } = new();
}