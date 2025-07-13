using System.ComponentModel.DataAnnotations;

namespace EbWeb.Models.InputModels;
public class AnomalieReportInputModel
{
    [Required]
    public int Nag { get; set; }
    [Required]
    public required string UserName { get; set; }
    [Required]
    public DateTime Date { get; set; }
}
