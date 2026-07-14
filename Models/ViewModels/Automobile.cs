namespace EbWeb.Models.ViewModels;

public class Automobile
{
    private int Colore { get; set; }
    public string Marca { get; set; }
    public string Modello { get; set; }

    public Automobile(int colore)
    {
        Colore = colore;
    }

    public void CambiaColore(int nuovoColore)
    {
        Colore = nuovoColore;
    }
}

public class Test
{
    public void Prova()
    {
        Automobile auto = new Automobile(20)
        {
            Marca = "Fiat",
            Modello = "Panda"
        };

        auto.CambiaColore(15);
    }
}
