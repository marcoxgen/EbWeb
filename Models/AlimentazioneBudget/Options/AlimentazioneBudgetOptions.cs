namespace EbWeb.Models.AlimentazioneBudget.Options;

public class AlimentazioneBudgetOptions
{
    public int PerPage { get; set; } = 10;
    public int CommandTimeout { get; set; } = 900;
    public OrderOptions Order { get; set; } = new();
}

public class OrderOptions
{
    public string By { get; set; } = string.Empty;
    public bool Ascending { get; set; }
    public List<string> Allow { get; set; } = new();
}