namespace EbWeb.Models.ControlliUatu.ViewModels;

public class ControlloUatuViewModel
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
    public long NumeroRecord { get; set; }
    public DateTime? UltimoAggiornamento { get; set; }

    public string RowCssClass => Priorita switch
    {
        1 => "bg-danger text-white",
        2 => "bg-orange text-white",
        3 => "bg-yellow",
        4 => "bg-light-yellow",
        _ => ""
    };

    public string UltimoAggiornamentoFriendly
    {
        get
        {
            if (!UltimoAggiornamento.HasValue) return string.Empty;

            var diff = DateTime.Now - UltimoAggiornamento.Value;

            if(diff.TotalSeconds < 60)
                return $"{(int)diff.TotalSeconds} s";

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} m";

            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} h";

            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} g";

            return UltimoAggiornamento.Value.ToString("dd/MM/yyyy");
        }
    }
}