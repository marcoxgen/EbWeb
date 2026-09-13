using EbWeb.Models.InputModels;
using EbWeb.Models.Results;
using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application;

public interface IRevisioneService
{
    Task<ListViewModel<RevisioneViewModel>> GetRevisioniAsync(RevisioneListInputModel model);
    Task<List<RevisioneViewModel>> GetRevisioneAsync(int id);
    Task<int> EditNoteAsync(int id, string note);
    Task<CreatePdfResult> CreatePdfAsync(int nag);
}