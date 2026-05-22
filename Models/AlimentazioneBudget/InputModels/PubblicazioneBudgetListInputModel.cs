using Microsoft.AspNetCore.Mvc;
using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.AlimentazioneBudget.Options;

namespace EbWeb.Models.AlimentazioneBudget.InputModels;

[ModelBinder(BinderType = typeof(PubblicazioneBudgetListInputModelBinder))]
public class PubblicazioneBudgetListInputModel
{
    public PubblicazioneBudgetListInputModel(int id_pubblicazione, int page, string orderby, bool ascending, int limit, AlimentazioneBudgetOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        Id_Pubblicazione = id_pubblicazione > 0 ? id_pubblicazione : (int?)null;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }
    public int? Id_Pubblicazione { get; }
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}