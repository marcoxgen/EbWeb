namespace EbWeb.Models.Results;

public class CreatePdfResult
{
    public bool Success { get; init; }

    public byte[]? Pdf { get; init; }

    public List<string> CampiMancanti { get; init; } = [];
}