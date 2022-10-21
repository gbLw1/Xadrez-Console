using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Peao : Peca
{
    public Peao(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

    public override string ToString() => "P";

    bool ExisteInimigo(Posicao posicao)
        => Tabuleiro.Peca(posicao) is not null
        && Tabuleiro.Peca(posicao).Cor != Cor;

    bool Livre(Posicao posicao)
        => Tabuleiro.Peca(posicao) is null;

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new(0, 0);

        if (Cor == Cor.Branca)
        {
            pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna);
            if (Tabuleiro.PosicaoValida(pos) && Livre(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
            if (Tabuleiro.PosicaoValida(pos) && Livre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tabuleiro.PosicaoValida(pos) && ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tabuleiro.PosicaoValida(pos) && ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

        }
        else
        {
            pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna);
            if (Tabuleiro.PosicaoValida(pos) && Livre(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
            if (Tabuleiro.PosicaoValida(pos) && Livre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tabuleiro.PosicaoValida(pos) && ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tabuleiro.PosicaoValida(pos) && ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;
        }

        return matriz;
    }
}