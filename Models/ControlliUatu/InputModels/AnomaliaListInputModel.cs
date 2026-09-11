using EbWeb.Customizations.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace EbWeb.Models.ControlliUatu.InputModels;

[ModelBinder(BinderType = typeof(AnomaliaListInputModelBinder))]
public class AnomaliaListInputModel
{
    public AnomaliaListInputModel(int? idSource, int page, int limit)
    {
        IdSource = idSource;
        Page = Math.Max(1, page);
        Limit = Math.Max(1, limit);
        Offset = (Page - 1) * Limit;
    }

    public int? IdSource { get; }

    public int Page { get; }
    public int Limit { get; }
    public int Offset { get; }
}