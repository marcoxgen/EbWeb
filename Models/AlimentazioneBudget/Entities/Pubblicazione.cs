using DocumentFormat.OpenXml.ExtendedProperties;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbWeb.Models.AlimentazioneBudget.Entities;

public partial class Pubblicazione
{
    public int Id_Pubblicazione { get; set; }
    public char Tipo_Pubblicazione_Cod { get; set; } = default!;
    public DateOnly Data_Riferimento { get; set; }
    public bool Flag_Stato { get; set; }
    public DateTime Data_Ultima_Lavorazione { get; set; }
    public string? Note { get; set; }
}