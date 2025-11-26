using EbWeb.Models.Entities;

namespace EbWeb.Models.ViewModels;

public class RichiestaPerfezionamentoViewModel
{
    public int IdRichiesta { get; set; }
    public DateTime DataRichiesta { get; set; }
    public int Filiale { get; set; }
    public string? Nag { get; set; }
    public string? Intestazione { get; set; }
    public string? StatoRichiesta { get; set; }
	public string? TipoRichiesta1 { get; set; }
    public string? Assegnatario1 { get; set; }
	public string? TipoRichiesta2 { get; set; }
    public string? Assegnatario2 { get; set; }
	public string? TipoRichiesta3 { get; set; }
    public string? Assegnatario3 { get; set; }
 }