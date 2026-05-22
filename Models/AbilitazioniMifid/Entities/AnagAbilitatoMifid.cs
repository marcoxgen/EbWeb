namespace EbWeb.Models.AbilitazioniMifid.Entities;

public class AnagAbilitatoMifid
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
    public DateOnly? Data_fine_abilitazione_Mifid { get; set; }
    public DateOnly? Data_sospensione { get; set; }
    public DateOnly? Data_termine_sospensione { get; set; }
    public bool? Necessario_assessment { get; set; }
    public DateOnly? Data_superamento_assessment { get; set; }
    public DateOnly? Data_abilitazione_titoli { get; set; }
    public DateOnly? Data_fine_abilitazione_titoli { get; set; }
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
    public bool? Flag_Abilitato_Mifid { get; set; }
    public bool? Abilitato_Finance_WMP { get; set; }
    public bool? Escluso { get; set; }
    public DateOnly? Data_Ultimo_Aggiornamento { get; set; }
    public string? Lettera_Abilitazione_in_Supervisione { get; set; }
    public string? Lettera_abilitazione_temporanea { get; set; }
    public string? Lettera_supervisione { get; set; }
    public string? Lettera_sostituto_supervisore { get; set; }
    public string? Lettera_cessazione_supervisione { get; set; }
    public string? Lettera_cessazione_supervisore { get; set; }
    public string? Lettera_cessazione_sostituto_supervisore { get; set; }
    public DateOnly? Data_Invio_lettera_abilitazione_in_supervisione { get; set; }
    public DateOnly? Data_Invio_lettera_abilitazione_temporanea { get; set; }
    public DateOnly? Data_Invio_lettera_supervisione { get; set; }
    public DateOnly? Data_Invio_lettera_sostituto_supervisore { get; set; }
    public DateOnly? Data_Invio_lettera_cessazione_supervisione { get; set; }
    public DateOnly? Data_Invio_lettera_cessazione_supervisore { get; set; }
    public DateOnly? Data_Invio_lettera_cessazione_sostituto_supervisore { get; set; }
}