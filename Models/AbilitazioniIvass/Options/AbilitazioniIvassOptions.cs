using EbWeb.Models.Common.Options;

namespace EbWeb.Models.AbilitazioniIvass.Options;

public class AbilitazioniIvassOptions : SecurityOptions
{
    public int PerPage { get; set; }
    public bool Escluso { get; set; }
    public AbilitazioniIvassOrderOptions Order { get; set; } = default!;
}

public class AbilitazioniIvassOrderOptions
{
    public string By { get; set; } = default!;
    public bool Ascending { get; set; }
    public string[] Allow { get; set; } = [];
}