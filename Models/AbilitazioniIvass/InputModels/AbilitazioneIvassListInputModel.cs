using EbWeb.Customizations.ModelBinders;
using EbWeb.Models.AbilitazioniIvass.Options;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.AbilitazioniIvass.InputModels;

[ModelBinder(BinderType = typeof(AbilitazioneIvassListInputModelBinder))]
public class AbilitazioneIvassListInputModel
{
    public AbilitazioneIvassListInputModel(
        int? matricola,
        string? intestazione,
        string? descrUO,
        string? ruolo,
        bool? flagFormatoMifid,
        bool? flagAbilitatoFinanza,
        bool? abilitatoOperativitaIvass,
        int page,
        string orderby,
        bool ascending,
        int limit,
        AbilitazioniIvassOrderOptions orderOptions)
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
        FlagFormatoMifid = flagFormatoMifid;
        FlagAbilitatoFinanza = flagAbilitatoFinanza;
        AbilitatoOperativitaIvass = abilitatoOperativitaIvass;

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
    public bool? FlagFormatoMifid { get; }
    public bool? FlagAbilitatoFinanza { get; }
    public bool? AbilitatoOperativitaIvass { get; }

    public int Page { get; }
    public string OrderBy { get; }
    public bool Ascending { get; }

    public int Limit { get; }
    public int Offset { get; }
}