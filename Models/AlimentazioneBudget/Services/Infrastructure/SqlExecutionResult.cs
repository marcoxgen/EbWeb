using System;
using System.Data;
using System.Collections.Generic;

namespace EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;

public class SqlExecutionResult
{
    // Proprietà per il Frontend / Controller
    public bool Esito { get; set; }
    public string Messaggio { get; set; }
    public List<string> Colonne { get; set; } = new();
    public List<Dictionary<string, object>> Righe { get; set; } = new();

    // Proprietà usate dal servizio ADO.NET
    public List<string> Messages { get; set; } = new();
    public List<DataTable> ResultSets { get; set; } = new();

    // Helper rapidi per i controlli nel servizio
    public string MessagesText => string.Join(Environment.NewLine, Messages);
    public bool HasMessages => Messages.Count > 0;
    public bool HasResults => ResultSets.Count > 0 && ResultSets[0].Rows.Count > 0;
}