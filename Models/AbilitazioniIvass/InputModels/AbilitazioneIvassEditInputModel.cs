using System.ComponentModel.DataAnnotations;
using EbWeb.Models.AbilitazioniIvass.Entities;

namespace EbWeb.Models.AbilitazioniIvass.InputModels;
public class AbilitazioneIvassEditInputModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int Matricola { get; set; }

    public string? Intestazione { get; set; }

    [Display(Name = "Esame IVASS")]
    public DateOnly? DataEsame { get; set; }

    [Display(Name = "Abilitazione operativa")]
    public DateOnly? DataAbilitazioneOperativa { get; set; }

    [Display(Name = "Fine abilitazione operativa")]
    public DateOnly? DataFineAbilitazioneOperativa { get; set; }

    [Display(Name = "Fine abilitazione IVASS")]
    public DateOnly? DataAbilitazioneIvass { get; set; }

    [Display(Name = "Fine abilitazione IVASS")]
    public DateOnly? DataFineAbilitazioneIvass { get; set; }

    [Display(Name = "Sospensione")]
    public DateOnly? DataSospensione { get; set; }

    [Display(Name = "Termine sospensione")]
    public DateOnly? DataTermineSospensione { get; set; }

    [Display(Name = "Note")]
    public string? Note { get; set; }

    [Display(Name = "Formazione 2021")]
    public string? Formazione2021 { get; set; }

    [Display(Name = "Formazione 2022")]
    public string? Formazione2022 { get; set; }

    [Display(Name = "Formazione 2023")]
    public string? Formazione2023 { get; set; }

    [Display(Name = "Formazione 2024")]
    public string? Formazione2024 { get; set; }

    [Display(Name = "Formazione 2025")]
    public string? Formazione2025 { get; set; }

    [Display(Name = "Formazione 2026")]
    public string? Formazione2026 { get; set; }

    public static AbilitazioneIvassEditInputModel FromEntity(ElencoAbilitatoIvass abilitato)
    {
        return new AbilitazioneIvassEditInputModel {
            Id = abilitato.Id,
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DataEsame = abilitato.Data_esame,
            DataAbilitazioneOperativa = abilitato.Data_abilitazione_Operativa,
            DataFineAbilitazioneOperativa = abilitato.Data_fine_abilitazione_Operativa,
            DataAbilitazioneIvass = abilitato.Data_abilitazione_IVASS,
            DataFineAbilitazioneIvass = abilitato.Data_fine_abilitazione_IVASS,
            DataSospensione = abilitato.Data_sospensione,
            DataTermineSospensione = abilitato.Data_termine_sospensione,
            Note = abilitato.Note,
            Formazione2021 = abilitato.Formazione_2021,
            Formazione2022 = abilitato.Formazione_2022,
            Formazione2023 = abilitato.Formazione_2023,
            Formazione2024 = abilitato.Formazione_2024,
            Formazione2025 = abilitato.Formazione_2025,
            Formazione2026 = abilitato.Formazione_2026
        };
    }
}