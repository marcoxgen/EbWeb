using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.InputModels;

[ModelBinder(BinderType = typeof(AgendaStipulaListInputModelBinder))]
public class AgendaStipulaListInputModel
{
    public AgendaStipulaListInputModel(int id_richiesta, int page, string orderby, bool ascending, int limit, AgendaStipuleOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        Id_Richiesta = id_richiesta > 0 ? id_richiesta : (int?)null;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }
    public int? Id_Richiesta { get; }
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}