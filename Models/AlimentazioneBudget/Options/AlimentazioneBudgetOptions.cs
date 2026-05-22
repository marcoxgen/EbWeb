namespace EbWeb.Models.AlimentazioneBudget.Options;

public class AlimentazioneBudgetOptions
{
    public int PerPage { get; set; }
    public AlimentazioneBudgetOrderOptions Order { get; set; } = default!;
}

public class AlimentazioneBudgetOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}