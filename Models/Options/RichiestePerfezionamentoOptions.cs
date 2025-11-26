namespace EbWeb.Models.Options;

   public class RichiestePerfezionamentoOptions
    {
    public int PerPage { get; set; }
    public int InHome { get; set; }
    public RichiestePerfezionamentoOrderOptions Order { get; set; }
}

public class RichiestePerfezionamentoOrderOptions
{
    public string By { get; set; }
    public bool Ascending { get; set; }
    public string[] Allow { get; set; }
}