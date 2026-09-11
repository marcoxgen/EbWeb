namespace EbWeb.Models.ControlliUatu.Options;

public class ControlliUatuOptions
{
    public int PerPage { get; set; }
    public ControlliUatuOrderOptions Order { get; set; } = default!;
}

public class ControlliUatuOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}