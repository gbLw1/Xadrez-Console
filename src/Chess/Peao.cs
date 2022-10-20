using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Peao : Peca
{
    // private readonly PartidaDeXadrez partida;

    // public Peao(Tabuleiro tabuleiro,
    //     Cor cor,
    //     PartidaDeXadrez partida)
    //     : base(tabuleiro, cor)
    //     => this.partida = partida;

    public Peao(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    public override string ToString() => "P";

    bool DestinoExisteInimigo(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is not null && Tabuleiro.Peca(posicao).Cor != Cor;
    }

    bool DestinoLivre(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is null;
    }

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new (0, 0);

        if (Cor == Cor.Branca)
        {
            pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna);
            if (DestinoLivre(pos)) matriz[pos.Linha, pos.Coluna] = true;


            // pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
            // var p2 = new Posicao(Posicao.Linha - 1, Posicao.Coluna);
            // if (DestinoLivre(p2) && DestinoLivre(pos) && QtdeMovimentos == 0)
            //     matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
            if (DestinoLivre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;


            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (DestinoExisteInimigo(pos)) matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (DestinoExisteInimigo(pos)) matriz[pos.Linha, pos.Coluna] = true;


            // // #jogada especial en passant
            // if (Posicao.Linha == 3)
            // {
            //     Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
            //     if (DestinoExisteInimigo(esquerda) && Tabuleiro.Peca(esquerda) == partida.vulneravelEnPassant)
            //     {
            //         matriz[esquerda.Linha - 1, esquerda.Coluna] = true;
            //     }
            //     Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
            //     if (DestinoExisteInimigo(direita) && Tabuleiro.Peca(direita) == partida.vulneravelEnPassant)
            //     {
            //         matriz[direita.Linha - 1, direita.Coluna] = true;
            //     }
            // }
        }
        else
        {
            pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna);
            if (DestinoLivre(pos)) matriz[pos.Linha, pos.Coluna] = true;

            // pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
            // Posicao p2 = new Posicao(Posicao.Linha + 1, Posicao.Coluna);
            // if (DestinoLivre(p2) && DestinoLivre(pos) && QtdeMovimentos == 0)
            //     matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
            if (DestinoLivre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (DestinoExisteInimigo(pos)) matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (DestinoExisteInimigo(pos)) matriz[pos.Linha, pos.Coluna] = true;


            // // #jogadaespecial en passant
            // if (Posicao.Linha == 4)
            // {
            //     Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
            //     if (DestinoExisteInimigo(esquerda) && Tabuleiro.Peca(esquerda) == partida.vulneravelEnPassant)
            //     {
            //         matriz[esquerda.Linha + 1, esquerda.Coluna] = true;
            //     }
            //     Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
            //     if (DestinoExisteInimigo(direita) && Tabuleiro.Peca(direita) == partida.vulneravelEnPassant)
            //     {
            //         matriz[direita.Linha + 1, direita.Coluna] = true;
            //     }
            // }
        }

        return matriz;
    }
}