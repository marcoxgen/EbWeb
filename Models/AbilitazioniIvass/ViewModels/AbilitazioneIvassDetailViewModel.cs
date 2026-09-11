using DocumentFormat.OpenXml.Drawing.Charts;
using EbWeb.Models.AbilitazioniIvass.Entities;
using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.AbilitazioniIvass.ViewModels;

public class AbilitazioneIvassDetailViewModel
{
    public int Id { get; set; }
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? CodiceFiscale { get; set; }
    public string? Ruolo { get; set; }
    public bool? FlagFormatoMifid { get; set; }
    public bool? FlagAbilitatoFinanza { get; set; }
    public DateOnly? DataAbilitazioneIvass { get; set; }
    public DateOnly? DataFineAbilitazioneIvass { get; set; }
    public DateOnly? DataEsame { get; set; }
    public DateOnly? DataAbilitazioneOperativa { get; set; }
    public DateOnly? DataFineAbilitazioneOperativa { get; set; }
    public DateOnly? DataSospensione { get; set; }
    public DateOnly? DataTermineSospensione { get; set; }
    public string? Formazione2021 { get; set; }
    public string? Formazione2022 { get; set; }
    public string? Formazione2023 { get; set; }
    public string? Formazione2024 { get; set; }
    public string? Formazione2025 { get; set; }
    public string? Formazione2026 { get; set; }
    public string? Note { get; set; }
    public bool? AbilitatoOperativitaIvass { get; set; }
    public DateOnly? DataUltimoAggiornamento { get; set; }
    public bool? Escluso { get; set; }
    public string? LetteraNomina { get; set; }
    public string? LetteraRevoca { get; set; }

    public static AbilitazioneIvassDetailViewModel FromEntity(ElencoAbilitatoIvass abilitato)
    {
        return new AbilitazioneIvassDetailViewModel {
            Id = abilitato.Id,
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DescrUO = abilitato.Descr_UO,
            CodiceFiscale = abilitato.Codice_Fiscale,
            Ruolo = abilitato.Ruolo,
            FlagFormatoMifid = abilitato.Flag_Formato_Mifid,
            FlagAbilitatoFinanza = abilitato.Flag_Abilitato_Finanza,
            DataAbilitazioneIvass = abilitato.Data_abilitazione_IVASS,
            DataFineAbilitazioneIvass = abilitato.Data_fine_abilitazione_IVASS,
            DataEsame = abilitato.Data_esame,
            DataAbilitazioneOperativa = abilitato.Data_abilitazione_Operativa,
            DataFineAbilitazioneOperativa = abilitato.Data_fine_abilitazione_Operativa,
            DataSospensione = abilitato.Data_sospensione,
            DataTermineSospensione = abilitato.Data_termine_sospensione,
            Formazione2021 = abilitato.Formazione_2021,
            Formazione2022 = abilitato.Formazione_2022,
            Formazione2023 = abilitato.Formazione_2023,
            Formazione2024 = abilitato.Formazione_2024,
            Formazione2025 = abilitato.Formazione_2025,
            Formazione2026 = abilitato.Formazione_2026,
            Note = abilitato.Note,
            AbilitatoOperativitaIvass = abilitato.Abilitato_Operativita_Ivass,
            DataUltimoAggiornamento = abilitato.Data_Ultimo_Aggiornamento,
            Escluso = abilitato.Escluso,
            LetteraNomina = abilitato.Lettera_Nomina,
            LetteraRevoca = abilitato.Lettera_Revoca
        };
    }
}