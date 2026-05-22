namespace EbWeb.Models.Common.ViewModels;

public class ListViewModel<T>
{
    public List<T> Results { get; set; } = new();
    public int TotalCount { get; set; }
}