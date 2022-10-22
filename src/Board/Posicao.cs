namespace Xadrez_Console.Board;

public class Posicao
{
    public int Linha { get; set; }
    public int Coluna { get; set; }

    public Posicao(int linha, int coluna)
        => (Linha, Coluna) = (linha, coluna);

    public void DefinirValores(int linha, int coluna)
        => (Linha, Coluna) = (linha, coluna);

    public override string ToString()
        => $"{Linha}, {Coluna}";
}