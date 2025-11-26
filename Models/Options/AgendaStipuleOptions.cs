namespace EbWeb.Models.Options
{
    public class AgendaStipuleOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }
        public AgendaStipuleOrderOptions Order { get; set; }
    }

    public class AgendaStipuleOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}
