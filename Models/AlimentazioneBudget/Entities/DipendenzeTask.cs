namespace EbWeb.Models.AlimentazioneBudget.Entities;

public partial class DipendenzeTask
{
    public int Id_Dipendenza { get; set; }
    public int Id_Task_Successore { get; set; }
    public int Id_Task_Predecessore { get; set; }
    public char Tipo_Pubblicazione_Cod { get; set; }
}
