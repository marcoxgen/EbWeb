namespace EbWeb.Models.AbilitazioniIvass.Entities;

public class ElencoAbilitatoIvass
{
    public int Id { get; set; }
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? Descr_UO { get; set; }
    public string? Codice_Fiscale { get; set; }
    public string? Ruolo { get; set; }
    public bool? Flag_Formato_Mifid { get; set; }
    public bool? Flag_Abilitato_Finanza { get; set; }
    public DateOnly? Data_abilitazione_IVASS { get; set; }
    public DateOnly? Data_fine_abilitazione_IVASS { get; set; }
    public DateOnly? Data_esame { get; set; }
    public DateOnly? Data_abilitazione_Operativa { get; set; }
    public DateOnly? Data_fine_abilitazione_Operativa { get; set; }
    public DateOnly? Data_sospensione { get; set; }
    public DateOnly? Data_termine_sospensione { get; set; }
    public string? Formazione_2021 { get; set; }
    public string? Formazione_2022 { get; set; }
    public string? Formazione_2023 { get; set; }
    public string? Formazione_2024 { get; set; }
    public string? Formazione_2025 { get; set; }
    public string? Formazione_2026 { get; set; }
    public string? Note { get; set; }
    public bool? Abilitato_Operativita_Ivass { get; set; }
    public DateOnly? Data_Ultimo_Aggiornamento { get; set; }
    public bool? Escluso { get; set; }
    public string? Lettera_Nomina { get; set; }
    public string? Lettera_Revoca { get; set; }
}