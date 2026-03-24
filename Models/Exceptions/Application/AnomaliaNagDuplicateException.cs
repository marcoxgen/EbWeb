namespace EbWeb.Models.Exceptions.Application;

public class AnomaliaNagDuplicateException : Exception
{
    public AnomaliaNagDuplicateException(int nag, Exception innerException) : base($"Nag anomalia '{nag}' duplicato", innerException)
    {        
    }
}