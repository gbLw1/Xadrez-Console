using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Cavalo : Peca
{

    public Cavalo(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    public override string ToString() => "C";

    bool PodeMover(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is null || Tabuleiro.Peca(posicao).Cor != Cor;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new (0, 0);

        pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna - 2);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha - 2, Posicao.Coluna - 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha - 2, Posicao.Coluna + 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna + 2);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna + 2);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha + 2, Posicao.Coluna + 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha + 2, Posicao.Coluna - 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna - 2);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        return matriz;
    }
}