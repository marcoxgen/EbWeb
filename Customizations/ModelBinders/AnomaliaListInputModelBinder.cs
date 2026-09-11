using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using EbWeb.Models.ControlliUatu.InputModels;
using EbWeb.Models.ControlliUatu.Options;

namespace EbWeb.Customizations.ModelBinders;

public class AnomaliaListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<ControlliUatuOptions> _options;

    public AnomaliaListInputModelBinder(IOptionsMonitor<ControlliUatuOptions> options)
    {
        _options = options;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var idSourceValue = bindingContext.ValueProvider.GetValue("idSource").FirstValue;
        int.TryParse(idSourceValue, out int idSource);

        var pageValue = bindingContext.ValueProvider.GetValue("page").FirstValue;
        int page = int.TryParse(pageValue, out var parsedPage) ? parsedPage : 1;

        var options = _options.CurrentValue;
        int limit = options.PerPage > 0 ? options.PerPage : 15;

        var inputModel = new AnomaliaListInputModel(idSource, page, limit);

        bindingContext.Result = ModelBindingResult.Success(inputModel);
        return Task.CompletedTask;
    }
}