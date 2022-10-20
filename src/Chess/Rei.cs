using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Rei : Peca
{
    public Rei(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    bool PodeMover(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is null || Tabuleiro.Peca(posicao).Cor != Cor;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        // Possíveis posições
        Posicao pos = new Posicao(0, 0);

        // acima
        pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // ne
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // direita
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // se
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // abaixo
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // so
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // esquerda
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        // no
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
        if (PodeMover(pos)) matriz[pos.Linha, pos.Coluna] = true;

        return matriz;
    }

    public override string ToString() => "R";
}