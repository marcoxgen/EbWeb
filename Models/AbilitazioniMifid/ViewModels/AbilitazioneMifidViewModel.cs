using EbWeb.Models.AbilitazioniMifid.Entities;

namespace EbWeb.Models.AbilitazioniMifid.ViewModels;

public class AbilitazioneMifidViewModel
{
    public int? Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? Ruolo { get; set; }
    public bool? FlagAbilitatoMifid { get; set; }
    public bool? AbilitatoFinanceWMP { get; set; }
    public DateOnly? DataSospensione { get; set; }
    public DateOnly? DataFineSupervisione { get; set; }

    public static AbilitazioneMifidViewModel FromEntity(AnagAbilitatoMifid abilitato)
    {
        return new AbilitazioneMifidViewModel {
            Matricola = abilitato.Matricola,
            Intestazione = abilitato.Intestazione,
            DescrUO = abilitato.Descr_UO,
            Ruolo = abilitato.Ruolo,
            FlagAbilitatoMifid = abilitato.Flag_Abilitato_Mifid,
            AbilitatoFinanceWMP = abilitato.Abilitato_Finance_WMP,
            DataSospensione = abilitato.Data_sospensione,
            DataFineSupervisione = abilitato.Data_fine_supervisione
        };
    }
}