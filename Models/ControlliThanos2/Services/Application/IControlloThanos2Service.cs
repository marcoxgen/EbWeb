using EbWeb.Models.ControlliThanos2.ViewModels;

namespace EbWeb.Models.ControlliThanos2.Services.Application;

public interface IControlloThanos2Service
{
    Task<List<ControlloThanos2ViewModel>> GetControlliThanos2Async();
}