using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Board;

public abstract class Peca
{
    public Posicao Posicao { get; set; } = default!;
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

    public void DecrementarQtdeMovimentos()
        => QtdeMovimentos--;

    public bool ExisteMovimentosPossiveis()
    {
        bool[,] matriz = MovimentosPossiveis();

        for (int i = 0; i < Tabuleiro.Linhas; i++)
        {
            for (int j = 0; j < Tabuleiro.Colunas; j++)
            {
                if (matriz[i,j]) return true;
            }
        }

        return false;
    }

    public bool PodeMoverPara(Posicao posicao)
        => MovimentosPossiveis()[posicao.Linha, posicao.Coluna];

    public abstract bool[,] MovimentosPossiveis();
}