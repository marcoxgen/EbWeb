namespace EbWeb.Models.Options;

public class AgendaStipuleOptions
{
    public int PerPage { get; set; }
    public AgendaStipuleOrderOptions Order { get; set; } = default!;
}

public class AgendaStipuleOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}