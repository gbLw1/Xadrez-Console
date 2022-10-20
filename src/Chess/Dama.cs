using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Dama : Peca
{

    public Dama(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    public override string ToString() => "D";

    bool PodeMover(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is null || Tabuleiro.Peca(posicao).Cor != Cor;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new(0, 0);

        // esquerda
        pos.DefinirValores(Posicao!.Linha, Posicao.Coluna - 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha, pos.Coluna - 1);
        }

        // direita
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha, pos.Coluna + 1);
        }

        // acima
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna);
        }

        // abaixo
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna);
        }

        // NO
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
        }

        // NE
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
        }

        // SE
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
        }

        // SO
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
        }

        return matriz;
    }
}