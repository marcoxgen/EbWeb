using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.InputModels;

[ModelBinder(BinderType = typeof(IstruttoriaListInputModelBinder))]
public class IstruttoriaListInputModel
{
    public IstruttoriaListInputModel(int nag, string cluster_pratica, bool? istruttore, int page, string orderby, bool ascending, int limit, IstruttorieOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }
        Nag = nag > 0 ? nag : (int?)null;
        Cluster_Pratica = cluster_pratica ?? "";
        Istruttore = istruttore;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }
    public int? Nag { get; }
    public string Cluster_Pratica { get; }
    public bool? Istruttore { get; }
    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}