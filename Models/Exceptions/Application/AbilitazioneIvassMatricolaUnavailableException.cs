namespace EbWeb.Models.Exceptions;

public class AbilitazioneIvassMatricolaUnavailableException : Exception
{
    public AbilitazioneIvassMatricolaUnavailableException(int matricola, Exception innerException) : base($"Matricola Ivass '{matricola}' già presente", innerException)
    {
    }
}