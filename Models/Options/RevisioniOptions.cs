namespace EbWeb.Models.Options
{
    public class RevisioniOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }
        public RevisioniOrderOptions Order { get; set; }
    }

    public class RevisioniOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}
