namespace MyCourse.Models.Exceptions;

public class AbilitazioneMifidMatricolaUnavailableException : Exception
{
    public AbilitazioneMifidMatricolaUnavailableException(int matricola, Exception innerException) : base($"Matricola Mifid '{matricola}' già presente", innerException)
    {
    }
}