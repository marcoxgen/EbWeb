namespace EbWeb.Models.Options
{
    public class SchedeBudgetOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }
        public SchedeBudgetOrderOptions Order { get; set; }
    }

    public class SchedeBudgetOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}
