namespace EbWeb.Models.Entities;

public partial class RichiestaPerfezionamento
{
    public int Id_Richiesta { get; set; }
	public DateTime Data_Richiesta { get; set; }
	public DateTime Data_Fine_Richiesta { get; set; }
	public string Stato_Richiesta { get; set; }
	public int Id_UO_Richiedente { get; set; }
	public string UO_Richiedente { get; set; }
	public string DataPrenotazione { get; set; }
    public string Nag { get; set; }
	public string Intestazione { get; set; }
	public short Filiale { get; set; }
	public string Comparto1 { get; set; }
	public string IdRichiesta1 { get; set; }
	public string FlagVeloce1 { get; set; }
	public string TipoRichiesta1 { get; set; }
	public string MinutiRichiesta1 { get; set; }
	public string Comparto2 { get; set; }
	public string IdRichiesta2 { get; set; }
	public string FlagVeloce2 { get; set; }
	public string TipoRichiesta2 { get; set; }
	public string MinutiRichiesta2 { get; set; }
	public string Comparto3 { get; set; }
	public string IdRichiesta3 { get; set; }
	public string FlagVeloce3 { get; set; }
	public string TipoRichiesta3 { get; set; }
	public string MinutiRichiesta3 { get; set; }
	public string stato_richiesta_Contratti { get; set; }
    public string Cod_Mediatore { get; set; }
	public string Oneri_Mediatore { get; set; }
	public string Est_Decurt_Fil_Mutuo { get; set; }
	public string Est_Decurt_Nr_Mutuo { get; set; }
	public string Est_Scelta_Polizze { get; set; }
	public string Decurt_Scelta_Polizze { get; set; }
}