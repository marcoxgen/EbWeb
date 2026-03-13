namespace EbWeb.Models.Entities;

public partial class SchedaBudget
{
    public string? Etichetta_Data { get; set; }
    public int ID_UO { get; set; }
    public string? Etichetta_UO { get; set; }
    public string? Tipo_Utente_Des { get; set; }
    public string? Ordinamento { get; set; }
    public string? Metrica_Des { get; set; }
    public string? Tipologia_Des { get; set; }
    public DateOnly? Data_Rif { get; set; }
    public double? Valore_Fine_Anno_Precedente { get; set; }
    public double? Rettifiche { get; set; }
    public double? Valore_Anno_Precedente_Rettificato { get; set; }
    public double? Valore_Stesso_Periodo_Anno_Precedente_Rettificato { get; set; }
    public double? Valore_Alla_Data_Rif { get; set; }
    public double? Flusso_Alla_Data { get; set; }
    public double? Obiettivo_Annuale { get; set; }
    public double? Valore_Da_Raggiungere_Annuale { get; set; }
    public string? Percentuale_Raggiungimento_Obiettivo_Annuale { get; set; }
    public double? Percentuale_Raggiungimento_Obiettivo_Annuale_Istogramma { get; set; }
    public double? Obiettivo_Mensile { get; set; }
    public double? Valore_Da_Raggiungere_Mensile { get; set; }
    public string? Percentuale_Raggiungimento_Obiettivo_Mensile { get; set; }
    public short? Punteggio { get; set; }
    public byte? Ad_Obiettivo { get; set; }
}
