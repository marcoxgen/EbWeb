using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.InputModels;

[ModelBinder(BinderType = typeof(SchedaBudgetListInputModelBinder))]
public class SchedaBudgetListInputModel
{
    public SchedaBudgetListInputModel(string etichettaSelezionata, int page, string orderby, bool ascending, int limit, SchedeBudgetOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        EtichettaSelezionata = etichettaSelezionata;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }
    public string? EtichettaSelezionata { get; set; } = string.Empty;
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}	