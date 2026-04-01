using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.InputModels;

[ModelBinder(BinderType = typeof(AbilitazioneMifidListInputModelBinder))]
public class AbilitazioneMifidListInputModel
{
    public AbilitazioneMifidListInputModel(string? search, int page, string orderby, bool ascending, int limit, bool? escluso, AbilitazioniMifidOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        Search = search ?? "";
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
        Escluso = escluso;
    }
    public string? Search { get; }
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }

    public bool? Escluso { get; }
}