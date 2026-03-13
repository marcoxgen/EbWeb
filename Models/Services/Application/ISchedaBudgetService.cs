using EbWeb.Models.InputModels;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface ISchedaBudgetService
{
    Task<List<string>> GetEtichetteDataAsync();
    Task<ListViewModel<SchedaBudgetViewModel>> GetSchedeBudgetAsync(SchedaBudgetListInputModel model);
}