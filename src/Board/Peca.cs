using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Board;

public abstract class Peca
{
    public Posicao? Posicao { get; set; }
    public Cor Cor { get; protected set; }
    public int QtdeMovimentos { get; protected set; }
    public Tabuleiro Tabuleiro { get; protected set; }

    public Peca(Tabuleiro tabuleiro, Cor cor)
    {
        Tabuleiro = tabuleiro;
        Cor = cor;
        QtdeMovimentos = 0;
    }

    public void IncrementarQtdeMovimentos()
        => QtdeMovimentos++;

    public abstract bool[,] MovimentosPossiveis();
}