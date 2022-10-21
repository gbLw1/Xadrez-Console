using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Rei : Peca
{
    private readonly PartidaDeXadrez _partida;

    public Rei(Tabuleiro tabuleiro,
               Cor cor,
               PartidaDeXadrez partida)
        : base(tabuleiro, cor)
        => _partida = partida;

    public override string ToString() => "R";

    bool PodeMover(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is null || Tabuleiro.Peca(posicao).Cor != Cor;
    }

    bool TesteTorreParaRoque(Posicao posicao)
    {
        Peca p = Tabuleiro.Peca(posicao);
        return p is not null && p is Torre && p.Cor == Cor && p.QtdeMovimentos == 0;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        // Possíveis posições
        Posicao pos = new(0, 0);

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

        // # Jogada especial - roque
        if (QtdeMovimentos == 0 && _partida.Xeque is false)
        {
            // roque pequeno
            Posicao posT1 = new(Posicao.Linha, Posicao.Coluna + 3);
            if (TesteTorreParaRoque(posT1))
            {
                Posicao p1 = new(Posicao.Linha, Posicao.Coluna + 1);
                Posicao p2 = new(Posicao.Linha, Posicao.Coluna + 2);
                if (Tabuleiro.Peca(p1) is null && Tabuleiro.Peca(p2) is null)
                    matriz[Posicao.Linha, Posicao.Coluna + 2] = true;
            }

            // roque grande
            Posicao posT2 = new(Posicao.Linha, Posicao.Coluna - 4);
            if (TesteTorreParaRoque(posT2))
            {
                Posicao p1 = new(Posicao.Linha, Posicao.Coluna - 1);
                Posicao p2 = new(Posicao.Linha, Posicao.Coluna - 2);
                Posicao p3 = new(Posicao.Linha, Posicao.Coluna - 3);
                if (Tabuleiro.Peca(p1) is null
                    && Tabuleiro.Peca(p2) is null
                    && Tabuleiro.Peca(p3) is null)
                    matriz[Posicao.Linha, Posicao.Coluna - 2] = true;
            }
        }

        return matriz;
    }
}