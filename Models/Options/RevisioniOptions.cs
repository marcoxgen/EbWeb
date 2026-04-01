namespace EbWeb.Models.Options;

public class RevisioniOptions
{
    public int PerPage { get; set; }
    public RevisioniOrderOptions Order { get; set; } = default!;
}

public class RevisioniOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}