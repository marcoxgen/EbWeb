namespace EbWeb.Models.Entities;

public partial class AnagDipendenti
{
	public short ID { get; set; }
	public short Matricola_Int { get; set; }
	public string? Matricola_Char { get; set; }
	public int NAG { get; set; }
	public string? Iniziale_Cognome { get; set; }
	public string? Cognome { get; set; }
	public string? Nome { get; set; }
	public string? Codice_Fiscale { get; set; }
	public string? Intestazione_Sicra { get; set; }
	public string? Intestazione_Sicra_Matricola { get; set; }
	public string? Mail { get; set; }
	public string? Utenza_Dominio_BccSi { get; set; }
	public bool Stato_Dominio_BccSi_Cod { get; set; }
	public string? Stato_Dominio_BccSi_Des { get; set; }
	public string? Utenza_Dominio_EmilBanca { get; set; }
	public string? Flag_Thinsoft { get; set; }
	public long ID_Thinsoft { get; set; }
	public string? Azienda { get; set; }
	public string? ID_UO { get; set; }
	public string? Descr_UO { get; set; }
	public int Filiale_Cod { get; set; }
	public string? Filiale_Tipologia { get; set; }
	public string? Centro_Costo_SAP { get; set; }
	public int CAB { get; set; }
	public string? Ruolo { get; set; }
	public string? Mansione_Sicra_Cod { get; set; }
	public string? Mansione_Sicra_Des { get; set; }
	public byte? Livello_Di_Delibera { get; set; }
	public string? Voip { get; set; }
	public DateOnly? Data_Nascita { get; set; }
	public string? Comune_Nascita { get; set; }
	public string? Provincia_Nascita { get; set; }
	public string? Indirizzo_Residenza { get; set; }
	public string? Località_Residenza { get; set; }
	public string? Comune_Residenza { get; set; }
	public string? Provincia_Residenza { get; set; }
	public string? CAP_Residenza { get; set; }
	public string? Sesso { get; set; }
	public string? Gestore_Cod { get; set; }
	public int Portafoglio { get; set; }
	public int Tipo_Gestore_Cod { get; set; }
	public string? Tipo_Gestore_Des { get; set; }
	public int ID_Responsabile { get; set; }
	public short Matricola_Int_Responsabile { get; set; }
	public string? Cognome_Responsabile { get; set; }
	public string? Nome_Responsabile { get; set; }
	public DateOnly? Data_inizio { get; set; }
	public DateOnly? Data_Fine { get; set; }
	public string? Note { get; set; }
}