namespace EbWeb.Models.Options;

public class IstruttorieOptions
{
    public int PerPage { get; set; }
    public int InHome { get; set; }
    public IstruttorieOrderOptions Order { get; set; } = default!;
}

public class IstruttorieOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}