using EbWeb.Models.AlimentazioneBudget.Entities;
using System.Text.Json;

namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class PubblicazioneBudgetViewModel
{
    public int IdPubblicazione { get; set; }
    public char TipoPubblicazioneCod { get; set; }
    public string TipoPubblicazioneDes { get; set; }
    public DateOnly DataRiferimento { get; set; }
    public bool FlagStato { get; set; }
    public DateTime DataUltimaLavorazione { get; set; }
    public string? Note {  get; set; }
}