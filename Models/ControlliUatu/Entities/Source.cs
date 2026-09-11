namespace EbWeb.Models.ControlliUatu.Entities;

public class Source
{
    public int IdSource { get; set; }
    public string? Anomalia { get; set; }
    public string? NomeDatabase { get; set; }
    public string? NomeSchema { get; set; }
    public string? NomeOggetto { get; set; }
	public bool Abilitato { get; set; }
	public int Priorita { get; set; }
    public string? Eccezioni { get; set; }
    public string? Descrizione { get; set; }
    public string? Filtro { get; set; }
    public long NumeroRecord { get; set; }
    public DateTime UltimoAggiornamento { get; set; }
}