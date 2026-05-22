namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class ActionSaveInputModel
{
    public int IdAzione { get; set; }
    public string? Note { get; set; }
    public bool Esito { get; set; }
    public string? Messaggio { get; set; }
    public string? Risultati { get; set; }
}
