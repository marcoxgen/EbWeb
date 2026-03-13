namespace EbWeb.Models.Options
{
    public class AbilitazioniMifidOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }
        public AbilitazioniMifidOrderOptions Order { get; set; }
    }

    public class AbilitazioniMifidOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}
