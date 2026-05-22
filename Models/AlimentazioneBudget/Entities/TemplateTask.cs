using System.ComponentModel.DataAnnotations.Schema;

namespace EbWeb.Models.AlimentazioneBudget.Entities;

[Table("Task")]
public class TemplateTask
{
    public int Id_Task { get; set; }
	public int Ordine { get; set; }
	public string Descrizione { get; set; }
	public byte Livello { get; set; }
	public string? Comando { get; set; }
	public string? Nome_DataBase { get; set; }
	public bool Abilitato { get; set; }
}
