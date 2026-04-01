namespace EbWeb.Models.Options;

public class AbilitazioniMifidOptions
{
    public int PerPage { get; set; }
    public bool Escluso { get; set; }
    public AbilitazioniMifidOrderOptions Order { get; set; } = default!;
}

public class AbilitazioniMifidOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}
