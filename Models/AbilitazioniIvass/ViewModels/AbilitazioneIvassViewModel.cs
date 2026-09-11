using EbWeb.Models.AbilitazioniIvass.Entities;

namespace EbWeb.Models.AbilitazioniIvass.ViewModels;

public class AbilitazioneIvassViewModel
{
    public int Id { get; set; }
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? Ruolo { get; set; }
    public bool? FlagFormatoMifid { get; set; }
    public bool? FlagAbilitatoFinanza { get; set; }
    public bool? AbilitatoOperativitaIvass { get; set; }
    public DateOnly? DataSospensione { get; set; }
    public DateOnly? DataAbilitazioneIvass { get; set; }

    public static AbilitazioneIvassViewModel FromEntity(ElencoAbilitatoIvass abilitato)
    {
        return new AbilitazioneIvassViewModel {
            Id = abilitato.Id,
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DescrUO = abilitato.Descr_UO,
            Ruolo = abilitato.Ruolo,
            FlagFormatoMifid = abilitato.Flag_Formato_Mifid,
            FlagAbilitatoFinanza = abilitato.Flag_Abilitato_Finanza,
            AbilitatoOperativitaIvass = abilitato.Abilitato_Operativita_Ivass,
            DataSospensione = abilitato.Data_sospensione,
            DataAbilitazioneIvass = abilitato.Data_abilitazione_IVASS
        };
    }
}