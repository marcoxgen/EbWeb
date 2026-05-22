using EbWeb.Models.AbilitazioniMifid.Entities;
using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AbilitazioniMifid.ViewModels;

public class AbilitazioneMifidDetailViewModel
{
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? CodiceFiscale { get; set; }
    public string? Ruolo { get; set; }
    public string? TitoloStudio { get; set; }
    public string? TitoloStudioMifid { get; set; }
    public int? MesiPeriodoSupervisione { get; set; }
    public DateOnly? DataConseguimentoTitoloStudio { get; set; }
    public DateOnly? DataAbilitazioneMifid { get; set; }
    public DateOnly? DataFineAbilitazioneMifid { get; set; }
    public DateOnly? DataSospensione { get; set; }
    public DateOnly? DataTermineSospensione { get; set; }
    public bool? NecessarioAssessment { get; set; }
    public DateOnly? DataSuperamentoAssessment { get; set; }
    public DateOnly? DataAbilitazioneTitoli { get; set; }
    public DateOnly? DataFineAbilitazioneTitoli { get; set; }
    public int? AnniEsperienzaAdeguata { get; set; }
    public DateOnly? DataInizioSupervisione { get; set; }
    public DateOnly? DataFineSupervisione { get; set; }
    public int? MatricolaSupervisore { get; set; }
    public string? IntestazioneSupervisore { get; set; }
    public int? MatricolaSostitutoSupervisore { get; set; }
    public string? IntestazioneSostitutoSupervisore { get; set; }
    public string? Formazione2024 { get; set; }
    public string? Formazione2025 { get; set; }
    public string? Note { get; set; }
    public bool? FlagAbilitatoMifid { get; set; }
    public bool? AbilitatoFinanceWMP { get; set; }
    public bool? Escluso { get; set; }
    public DateOnly? DataUltimoAggiornamento { get; set; }
    public string? LetteraAbilitazioneSupervisione { get; set; }
    public string? LetteraAbilitazioneTemporanea { get; set; }
    [Display(Name = "Nomina supervisore")]
    public string? LetteraSupervisione { get; set; }
    public string? LetteraSostitutoSupervisore { get; set; }
    public string? LetteraCessazioneSupervisione { get; set; }
    public string? LetteraCessazioneSupervisore { get; set; }
    public string? LetteraCessazioneSostitutoSupervisore { get; set; }
    public DateOnly? DataInvioLetteraAbilitazioneSupervisione { get; set; }
    public DateOnly? DataInvioLetteraAbilitazioneTemporanea { get; set; }
    public DateOnly? DataInvioLetteraSupervisione { get; set; }
    public DateOnly? DataInvioLetteraSostitutoSupervisore { get; set; }
    public DateOnly? DataInvioLetteraCessazioneSupervisione { get; set; }
    public DateOnly? DataInvioLetteraCessazioneSupervisore { get; set; }
    public DateOnly? DataInvioLetteraCessazioneSostitutoSupervisore { get; set; }

    public static AbilitazioneMifidDetailViewModel FromEntity(AnagAbilitatoMifid abilitato)
    {
        return new AbilitazioneMifidDetailViewModel {
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DescrUO = abilitato.Descr_UO,
            CodiceFiscale = abilitato.Codice_Fiscale,
            Ruolo = abilitato.Ruolo,
            TitoloStudio = abilitato.Titolo_di_studio,
            TitoloStudioMifid = abilitato.Titolo_di_studio_Mifid,
            MesiPeriodoSupervisione = abilitato.Mesi_periodo_di_supervisione,
            DataConseguimentoTitoloStudio = abilitato.Data_conseguimento_titolo_di_studio,
            DataAbilitazioneMifid = abilitato.Data_abilitazione_Mifid,
            DataFineAbilitazioneMifid = abilitato.Data_fine_abilitazione_Mifid,
            DataSospensione = abilitato.Data_sospensione,
            DataTermineSospensione = abilitato.Data_termine_sospensione,
            NecessarioAssessment = abilitato.Necessario_assessment,
            DataSuperamentoAssessment = abilitato.Data_superamento_assessment,
            DataAbilitazioneTitoli = abilitato.Data_abilitazione_titoli,
            DataFineAbilitazioneTitoli = abilitato.Data_fine_abilitazione_titoli,
            AnniEsperienzaAdeguata = abilitato.Esperienza_adeguata_in_anni,
            DataInizioSupervisione = abilitato.Data_inizio_supervisione,
            DataFineSupervisione = abilitato.Data_fine_supervisione,
            MatricolaSupervisore = abilitato.Matricola_supervisore,
            IntestazioneSupervisore = abilitato.Intestazione_supervisore,
            MatricolaSostitutoSupervisore = abilitato.Matricola_sostituto_supervisore,
            IntestazioneSostitutoSupervisore = abilitato.Intestazione_sostituto_supervisore,
            Formazione2024 = abilitato.Formazione_2024,
            Formazione2025 = abilitato.Formazione_2025,
            Note = abilitato.Note,
            FlagAbilitatoMifid = abilitato.Flag_Abilitato_Mifid,
            AbilitatoFinanceWMP = abilitato.Abilitato_Finance_WMP,
            Escluso = abilitato.Escluso,
            DataUltimoAggiornamento = abilitato.Data_Ultimo_Aggiornamento,
            LetteraAbilitazioneSupervisione = abilitato.Lettera_Abilitazione_in_Supervisione,
            LetteraAbilitazioneTemporanea = abilitato.Lettera_abilitazione_temporanea,
            LetteraSupervisione = abilitato.Lettera_supervisione,
            LetteraSostitutoSupervisore = abilitato.Lettera_sostituto_supervisore,
            LetteraCessazioneSupervisione = abilitato.Lettera_cessazione_supervisione,
            LetteraCessazioneSupervisore = abilitato.Lettera_cessazione_supervisore,
            LetteraCessazioneSostitutoSupervisore = abilitato.Lettera_cessazione_sostituto_supervisore,
            DataInvioLetteraAbilitazioneSupervisione = abilitato.Data_Invio_lettera_abilitazione_in_supervisione,
            DataInvioLetteraAbilitazioneTemporanea = abilitato.Data_Invio_lettera_abilitazione_temporanea,
            DataInvioLetteraSupervisione = abilitato.Data_Invio_lettera_supervisione,
            DataInvioLetteraSostitutoSupervisore = abilitato.Data_Invio_lettera_sostituto_supervisore,
            DataInvioLetteraCessazioneSupervisione = abilitato.Data_Invio_lettera_cessazione_supervisione,
            DataInvioLetteraCessazioneSupervisore = abilitato.Data_Invio_lettera_cessazione_supervisore,
            DataInvioLetteraCessazioneSostitutoSupervisore = abilitato.Data_Invio_lettera_cessazione_sostituto_supervisore  
         };
    }
}