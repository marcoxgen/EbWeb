using System.ComponentModel.DataAnnotations.Schema;

namespace EbWeb.Models.AlimentazioneBudget.Entities;

public class AzionePubblicazione
{
    public int Id_Azione { get; set; }
    public int Id_Pubblicazione { get; set; }
    public char TipoPub {  get; set; }
    public DateOnly DataRif {  get; set; }
    public int Id_Task { get; set; }
    public string? Descrizione { get; set; }
    public short Ordine { get; set; }
    public bool? Abilitato { get; set; }
    public string? Nome_Database { get; set; }
    public byte Livello { get; set; }
    public string? Comando { get; set; }
    public string? Istruzioni { get; set; }
    public bool Esito { get; set; }
    public DateTime? Data_Esecuzione { get; set; }
    public string? Risultati { get; set; }
    public string? Messaggio { get; set; }
    public string? Note { get; set; }
    public bool Flag_Esecuzione { get; set; }
}
