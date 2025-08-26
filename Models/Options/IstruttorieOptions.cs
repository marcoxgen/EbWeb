namespace EbWeb.Models.Options
{
    public class IstruttorieOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }
        public RevisioniOrderOptions Order { get; set; }
    }

    public class IstruttorieOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}
