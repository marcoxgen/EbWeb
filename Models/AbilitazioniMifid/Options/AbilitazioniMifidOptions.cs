using EbWeb.Models.Common.Options;

namespace EbWeb.Models.AbilitazioniMifid.Options;

public class AbilitazioniMifidOptions : SecurityOptions
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