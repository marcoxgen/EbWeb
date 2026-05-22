namespace EbWeb.Models.AlimentazioneBudget.Entities;

public class Azione
{
    public int Id_Azione { get; set; }
	public int Id_Pubblicazione { get; set; }
	public int Id_Task { get; set; }
	public bool Esito { get; set; }
	public DateTime? Data_Esecuzione { get; set; }
	public string? Messaggio { get; set; }
	public string? Note { get; set; }
}