using System.ComponentModel.DataAnnotations;
using EbWeb.Models.Entities;

namespace EbWeb.Models.InputModels;
public class AbilitazioneMifidEditInputModel
{
    [Required]
    public int Matricola { get; set; }

    public string? Intestazione { get; set; }

    [Display(Name = "Titolo di studio")]
    public string? TitoloStudio { get; set; }
    
    [Display(Name = "Titolo di studio MiFID")]
    public byte? TitoloStudioMifidCod { get; set; }

    [Display(Name = "Conseguimento titolo di studio")]
    public DateOnly? DataConseguimentoTitoloStudio { get; set; }

    [Display(Name = "Abilitazione MiFID")]
    public DateOnly? DataAbilitazioneMifid { get; set; }

    [Display(Name = "Inizio sospensione")]
    public DateOnly? DataSospensione { get; set; }

    [Display(Name = "Termine sospensione")]
    public DateOnly? DataTermineSospensione { get; set; }

    [Display(Name = "Necessario assessment")]
    public bool? NecessarioAssessment { get; set; }

    [Display(Name = "Superamento assessment")]
    public DateOnly? DataSuperamentoAssessment { get; set; }

    [Display(Name = "Abilitazione titoli")]
    public DateOnly? DataAbilitazioneTitoli { get; set; }

    [Display(Name = "Inizio supervisione")]
    public DateOnly? DataInizioSupervisione { get; set; }

    [Display(Name = "Fine supervisione")]
    public DateOnly? DataFineSupervisione { get; set; }

    [Display(Name = "Supervisore")]
    public int? MatricolaSupervisore { get; set; }

    [Display(Name = "Sostituto supervisore")]
    public int? MatricolaSostitutoSupervisore { get; set; }

    [Display(Name = "Formazione 2024")]
    public string? Formazione2024 { get; set; }

    [Display(Name = "Formazione 2025")]
    public string? Formazione2025 { get; set; }

    [Display(Name = "Note")]
    public string? Note { get; set; }

    [Display(Name = "Abilitazione WMP")]
    public bool? Abilitato_Finance_WMP { get; set; }

    [Display(Name = "Escludi")]
    public bool? Escluso { get; set; }

    public static AbilitazioneMifidEditInputModel FromEntity(AnagAbilitatoMifid abilitato)
    {
        return new AbilitazioneMifidEditInputModel {
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            TitoloStudio = abilitato.Titolo_di_studio,
            TitoloStudioMifidCod = abilitato.Titolo_di_studio_Mifid_Cod,
            DataConseguimentoTitoloStudio = abilitato.Data_conseguimento_titolo_di_studio,
            DataAbilitazioneMifid = abilitato.Data_abilitazione_Mifid,
            DataSospensione = abilitato.Data_sospensione,
            DataTermineSospensione = abilitato.Data_termine_sospensione,
            NecessarioAssessment = abilitato.Necessario_assessment,
            DataSuperamentoAssessment = abilitato.Data_superamento_assessment,
            DataAbilitazioneTitoli = abilitato.Data_abilitazione_titoli,
            DataInizioSupervisione = abilitato.Data_inizio_supervisione,
            DataFineSupervisione = abilitato.Data_fine_supervisione,
            MatricolaSupervisore = abilitato.Matricola_supervisore,
            MatricolaSostitutoSupervisore = abilitato.Matricola_sostituto_supervisore,
            Formazione2024 = abilitato.Formazione_2024,
            Formazione2025 = abilitato.Formazione_2025,
            Note = abilitato.Note,
            Abilitato_Finance_WMP = abilitato.Abilitato_Finance_WMP,
            Escluso = abilitato.Escluso
        };
    }
}