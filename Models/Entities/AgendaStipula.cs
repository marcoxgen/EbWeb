namespace EbWeb.Models.Entities;

public partial class AgendaStipula
{
        public int Id_Richiesta { get; set; }
		public DateTime Data_Richiesta { get; set; }
		public DateTime? Data_Fine_Richiesta { get; set; }
		public string? Stato_Richiesta { get; set; }
		public int? Id_UO_Richiedente { get; set; }
        public string? UO_Richiedente { get; set; }
        public string? Tipo_Richiesta { get; set; }
        public string? Data_Stipula_Selezionata { get; set; }
        public string? Stato_Stipula { get; set; }
        public string? Nag { get; set; }
        public string? Intestazione { get; set; }
        public string? Numero_Pratica { get; set; }
        public short Filiale { get; set; }
        public string? Linea_di_Credito { get; set; }
        public string? Descrizione_Prodotto { get; set; }
        public string? Importo_Deliberato { get; set; }
        public string? Stato_Fondo { get; set; }
        public string? Mutuo_SAL { get; set; }
        public string? Surroga { get; set; }
        public string? Art585 { get; set; }
        public string? Numero_Mutuo { get; set; }
        public string? Stipula_Prima13 { get; set; }
        public string? Mediatore_Convenzionato { get; set; }
        public string? Mediatore_Non_Convenzionato { get; set; }
        public string? Provvigione_Agenzia { get; set; }
        public string? Tipo_Atto { get; set; }
        public string? Assegnatario { get; set; }
}
