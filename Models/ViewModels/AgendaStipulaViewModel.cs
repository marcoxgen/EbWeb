using EbWeb.Models.Entities;

namespace EbWeb.Models.ViewModels;

public class AgendaStipulaViewModel
{
    public int IdRichiesta { get; set; }
    public DateTime DataRichiesta { get; set; }
    public string? Filiale { get; set; }
    public string? Nag { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrizioneProdotto { get; set; }
    public string? TipoAtto { get; set; }
    public string? ImportoDeliberato { get; set; }
    public string? MutuoSAL { get; set; }
    public string? NumeroMutuo { get; set; }
    public string? StatoFondo { get; set; }
    public string? StatoRichiesta { get; set; }
    public string? StatoStipula { get; set; }
    public string? Assegnatario { get; set; }
 }