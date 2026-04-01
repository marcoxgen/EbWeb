namespace EbWeb.Models.Entities;

public partial class AnagAbilitatoMifid
{
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? Descr_UO { get; set; }
    public string? Codice_Fiscale { get; set; }
    public string? Ruolo { get; set; }
    public string? Titolo_di_studio { get; set; }
    public string? Titolo_di_studio_Mifid { get; set; }
    public byte? Titolo_di_studio_Mifid_Cod { get; set; }
    public byte? Mesi_periodo_di_supervisione { get; set; }
    public DateOnly? Data_conseguimento_titolo_di_studio { get; set; }
    public DateOnly? Data_abilitazione_Mifid { get; set; }
    public DateOnly? Data_sospensione { get; set; }
    public DateOnly? Data_termine_sospensione { get; set; }
    public bool? Necessario_assessment { get; set; }
    public DateOnly? Data_superamento_assessment { get; set; }
    public DateOnly? Data_abilitazione_titoli { get; set; }
    public int? Esperienza_adeguata_in_anni { get; set; }
    public DateOnly? Data_inizio_supervisione { get; set; }
    public DateOnly? Data_fine_supervisione { get; set; }
    public int? Matricola_supervisore { get; set; }
    public string? Intestazione_supervisore { get; set; }
    public int? Matricola_sostituto_supervisore { get; set; }
    public string? Intestazione_sostituto_supervisore { get; set; }
    public string? Formazione_2024 { get; set; }
    public string? Formazione_2025 { get; set; }
    public string? Note { get; set; }
    public string? Flag_Abilitato_Mifid { get; set; }
    public bool? Abilitato_Finance_WMP { get; set; }
    public bool? Escluso { get; set; }
    public DateOnly? Data_Ultimo_Aggiornamento { get; set; }
    public string? Genera_Lettera_X { get; set; }
    public string? Genera_Lettera_Y { get; set; }
    public string? Genera_Lettera_Z { get; set; }
}
