namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class PubblicazioneBudgetViewModel
{
    public int IdPubblicazione { get; set; }
    public string TipoPubblicazioneCod { get; set; }
    public string TipoPubblicazioneDes { get; set; }
    public DateOnly DataRiferimento { get; set; }
    public bool FlagStato { get; set; }
    public DateTime DataUltimaLavorazione { get; set; }
}