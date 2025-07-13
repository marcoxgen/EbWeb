using System;
using System.Collections.Generic;

namespace EbWeb.Models.Entities;

public class AnomaliaRegistrazioneForzatura
{
    public int? NAG { get; set; }

    public string? Utente { get; set; }

    public DateTime? DataForzatura { get; set; }
}
