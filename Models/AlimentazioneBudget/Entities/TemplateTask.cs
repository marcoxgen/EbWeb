using System.ComponentModel.DataAnnotations.Schema;

[Table("Task")]
public class TemplateTask
{
    public int Id_Task { get; set; }
    public int Ordine { get; set; }
    public string? Descrizione { get; set; }
    public int Livello_Calcolato { get; set; }
    public string? Comando { get; set; }
    public string? Nome_Database { get; set; }
    public bool? Abilitato { get; set; }
    public string? Istruzioni { get; set; }
    public int? Id_Task_Padre { get; set; }
}