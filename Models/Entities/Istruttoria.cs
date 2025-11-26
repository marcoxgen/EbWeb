namespace EbWeb.Models.Entities;

public partial class Istruttoria
{
    public long? Numero_Pratica { get; set; }
    public int? Nag { get; set; }
    public string? Intestazione { get; set; }
    public string? Tipo_Controparte_Cod { get; set; }
    public string? Gestore { get; set; }
    public string? Organo_Deliberante_Cod { get; set; }
    public DateOnly? Data_Inserimento_Pratica { get; set; }
    public DateOnly? Data_Ultimo_Trasferimento { get; set; }
    public string? Ultimo_Cod_GoBack { get; set; }
    public DateOnly? Data_Ultimo_GoBack { get; set; }
    public DateOnly? Elimina_Code { get; set; }
    public string? Tipo_Istruttoria_Cod { get; set; }
    public string? Stato_Pratica_Cod { get; set; }
    public string? Grado_Rischio_Cod { get; set; }
    public string? Profilo_Antiriciclaggio { get; set; }
    public int? Rating { get; set; }
    public string? Cluster_Pratica { get; set; }
    public string? Istruttore { get; set; }
    public string? Note { get; set; }
    public string? Note_escalation_indicatori_bilancio { get; set; }
}
