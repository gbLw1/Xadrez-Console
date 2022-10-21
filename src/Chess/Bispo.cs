using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Bispo : Peca
{

    public Bispo(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    public override string ToString() => "B";

    bool PodeMover(Posicao posicao)
    {
        return Tabuleiro.Peca(posicao) is null || Tabuleiro.Peca(posicao).Cor != Cor;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new (0, 0);

        // NO
        pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna - 1);
        while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
        }

        // NE
        pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna + 1);
        while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
        }

        // SE
        pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna + 1);
        while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
        }

        // SO
        pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna - 1);
        while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
        }

        return matriz;
    }
}