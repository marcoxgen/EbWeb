using EbWeb.Models.Entities;

namespace EbWeb.Models.ViewModels;

public class AbilitazioneMifidViewModel
{
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? Ruolo { get; set; }
    public DateOnly? DataAbilitazioneMifid { get; set; }
    public string? FlagAbilitatoMifid { get; set; }
    public DateOnly? DataFineSupervisione { get; set; }
    public bool? Escluso{ get; set; }

    public static AbilitazioneMifidViewModel FromEntity(AnagAbilitatoMifid abilitato)
    {
        return new AbilitazioneMifidViewModel {
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DescrUO = abilitato.Descr_UO,
            Ruolo = string.IsNullOrWhiteSpace(abilitato.Ruolo) ? "Addetto" : abilitato.Ruolo,
            DataAbilitazioneMifid = abilitato.Data_abilitazione_Mifid,
            FlagAbilitatoMifid = abilitato.Flag_Abilitato_Mifid,
            DataFineSupervisione = abilitato.Data_fine_supervisione,
            Escluso = abilitato.Escluso
        };
    }
}