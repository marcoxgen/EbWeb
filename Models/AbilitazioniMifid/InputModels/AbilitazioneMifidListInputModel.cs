using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.AbilitazioniMifid.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.AbilitazioniMifid.InputModels;

[ModelBinder(BinderType = typeof(AbilitazioneMifidListInputModelBinder))]
public class AbilitazioneMifidListInputModel
{
    public AbilitazioneMifidListInputModel(
        int? matricola,
        string? intestazione,
        string? descrUO,
        string? ruolo,
        bool? flagAbilitatoMifid,
        bool? abilitatoFinanceWMP,
        int page,
        string orderby,
        bool ascending,
        int limit,
        AbilitazioniMifidOrderOptions orderOptions)
    {
        if (!orderOptions.Allow.Contains(orderby))
        {
            orderby = orderOptions.By;
            ascending = orderOptions.Ascending;
        }

        Matricola = matricola;
        Intestazione = intestazione ?? "";
        DescrUO = descrUO ?? "";
        Ruolo = ruolo ?? "";
        FlagAbilitatoMifid = flagAbilitatoMifid;
        AbilitatoFinanceWMP = abilitatoFinanceWMP;

        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        OrderBy = orderby;
        Ascending = ascending;

        Offset = (Page - 1) * Limit;
    }

    public int? Matricola { get; }
    public string? Intestazione { get; }
    public string? DescrUO { get; }
    public string? Ruolo { get; }
    public bool? FlagAbilitatoMifid { get; }
    public bool? AbilitatoFinanceWMP { get; }

    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}