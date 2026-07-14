namespace EbWeb.Models.ControlliThanos2.Options;

public class ControlliThanos2Options
{
    public int PerPage { get; set; }
    public ControlliThanos2OrderOptions Order { get; set; } = default!;
}

public class ControlliThanos2OrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}