using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.InputModels;

[ModelBinder(BinderType = typeof(AbilitazioneMifidListInputModelBinder))]
public class AbilitazioneMifidListInputModel
{
    public AbilitazioneMifidListInputModel(int id_matricola, int page, string orderby, bool ascending, int limit, AbilitazioniMifidOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        Id_Matricola = id_matricola > 0 ? id_matricola : (int?)null;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }
    public int? Id_Matricola { get; }
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}