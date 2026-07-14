using DocumentFormat.OpenXml.Wordprocessing;

namespace EbWeb.Models.AbilitazioniIvass.Entities;

public class AbilitatoIvass
{
    public AbilitatoIvass(int matricola)
    {
        Matricola = matricola;
        Escluso = false;
    }

    public int Id { get; set; }
    public int Matricola { get; set; }
    public DateOnly? Data_esame { get; set; }
	public DateOnly? Data_abilitazione_operativa { get; set; }
    public DateOnly? Data_fine_abilitazione_operativa { get; set; }
    public DateOnly? Data_abilitazione_IVASS { get; set; }
    public DateOnly? Data_fine_abilitazione_IVASS { get; set; }
    public DateOnly? Data_sospensione { get; set; }
    public DateOnly? Data_termine_sospensione { get; set; }
    public string? Note { get; set; }
	public string? Formazione_2021 { get; set; }
    public string? Formazione_2022 { get; set; }
	public string? Formazione_2023 { get; set; }
	public string? Formazione_2024 { get; set; }
	public string? Formazione_2025 { get; set; }
    public string? Formazione_2026 { get; set; }
    public DateOnly? Data_Ultimo_Aggiornamento { get; set; }
    public bool? Escluso { get; set; }

    public void ChangeMatricola(int matricola)
    {
        Matricola = matricola;
    }

    public void ChangeDataEsame(DateOnly? dataEsame)
    {
        Data_esame = dataEsame;
    }

    public void ChangeDataAbilitazioneOperativa(DateOnly? dataAbilitazioneOperativa)
    {
        Data_abilitazione_operativa = dataAbilitazioneOperativa;
    }

    public void ChangeDataFineAbilitazioneOperativa(DateOnly? dataFineAbilitazioneOperativa)
    {
        Data_fine_abilitazione_operativa = dataFineAbilitazioneOperativa;
    }

    public void ChangeDataAbilitazioneIvass(DateOnly? dataAbilitazioneIvass)
    {
        Data_abilitazione_IVASS = dataAbilitazioneIvass;
    }

    public void ChangeDataFineAbilitazioneIvass(DateOnly? dataFineAbilitazioneIvass)
    {
        Data_fine_abilitazione_IVASS = dataFineAbilitazioneIvass;
    }

    public void ChangeDataSospensione(DateOnly? dataSospensione)
    {
        Data_sospensione = dataSospensione;
    }

    public void ChangeDataTermineSospensione(DateOnly? dataTermineSospensione)
    {
        Data_termine_sospensione = dataTermineSospensione;
    }

    public void ChangeNote(string note)
    {
        Note = note;
    }

    public void ChangeFormazione2021(string? formazione2021)
    {
        Formazione_2021 = formazione2021;
    }

    public void ChangeFormazione2022(string? formazione2022)
    {
        Formazione_2022 = formazione2022;
    }

    public void ChangeFormazione2023(string? formazione2023)
    {
        Formazione_2023 = formazione2023;
    }

    public void ChangeFormazione2024(string? formazione2024)
    {
        Formazione_2024 = formazione2024;
    }

    public void ChangeFormazione2025(string? formazione2025)
    {
        Formazione_2025 = formazione2025;
    }

    public void ChangeFormazione2026(string? formazione2026)
    {
        Formazione_2026 = formazione2026;
    }

    public void ChangeDataUltimoAggiornamento()
    {
        Data_Ultimo_Aggiornamento = DateOnly.FromDateTime(DateTime.Now);
    }

    public void ChangeEscluso(bool? escluso)
    {
        Escluso = escluso;
    }
}