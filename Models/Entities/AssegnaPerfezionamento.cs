namespace EbWeb.Models.Entities;

public partial class AssegnaPerfezionamento
{
    public int Id_Richiesta { get; set; }
    public string? Assegnatario1 { get; set; }
    public string? Assegnatario2 { get; set; }
    public string? Assegnatario3 { get; set; }
    public DateTime Data { get; set; }
}
