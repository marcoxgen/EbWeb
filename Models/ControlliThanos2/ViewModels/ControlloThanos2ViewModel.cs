namespace EbWeb.Models.ControlliThanos2.ViewModels;

public class ControlloThanos2ViewModel
{
    public int IdSource { get; set; }

    public string? Anomalia { get; set; }

    public string? NameDatabase { get; set; }

    public string? NameSchema { get; set; }

    public string? NameOggetto { get; set; }

    public bool Abilitato { get; set; }

    public int Priorita { get; set; }

    public string? Eccezioni { get; set; }

    public string? Filtro { get; set; }

    public string? Descrizione { get; set; }

    public int NumeroRecord { get; set; }

    public DateTime UltimoAggiornamento { get; set; }
    
    public string RowCssClass => Priorita switch
    {
        1 => "bg-danger text-white",
        2 => "bg-orange text-white",
        3 => "bg-yellow",
        4 => "bg-light-yellow",
        _ => ""
    };
}