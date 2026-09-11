using EbWeb.Models.ControlliUatu.InputModels;

namespace EbWeb.Models.ControlliUatu.ViewModels;

public class AnomaliaListViewModel
{
    public string Anomalia { get; set; } = string.Empty;
    public bool FlagEccezioni { get; set; }
    public ListViewModel<AnomaliaViewModel> Anomalie { get; set; } = new();
    public AnomaliaListInputModel Input { get; set; } = default!;
}