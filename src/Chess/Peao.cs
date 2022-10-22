using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Peao : Peca
{
    private readonly PartidaDeXadrez _partida;

    public Peao(Tabuleiro tabuleiro,
        Cor cor,
        PartidaDeXadrez partida)
        : base(tabuleiro, cor)
        => _partida = partida;

    public override string ToString() => "P";

    bool ExisteInimigo(Posicao posicao)
    {
        if (Tabuleiro.PosicaoValida(posicao) is false) return false;
        return Tabuleiro.Peca(posicao) is not null && Tabuleiro.Peca(posicao).Cor != Cor;
    }

    bool Livre(Posicao posicao)
        => Tabuleiro.PosicaoValida(posicao)
        && Tabuleiro.Peca(posicao) is null;

    public override bool[,] MovimentosPossiveis()
    {
        bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

        Posicao pos = new(0, 0);

        if (Cor == Cor.Branca)
        {
            pos.DefinirValores(Posicao!.Linha - 1, Posicao.Coluna);
            if (Livre(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
            if (Livre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            // # Jogada especial - en passant
            if (Posicao.Linha == 3)
            {
                Posicao esquerda = new (Posicao.Linha, Posicao.Coluna - 1);
                if (ExisteInimigo(esquerda) && Tabuleiro.Peca(esquerda) == _partida.VulneravelEnPassant)
                    matriz[esquerda.Linha - 1, esquerda.Coluna] = true;
            
                Posicao direita = new (Posicao.Linha, Posicao.Coluna + 1);
                if (ExisteInimigo(direita) && Tabuleiro.Peca(direita) == _partida.VulneravelEnPassant)
                    matriz[direita.Linha - 1, direita.Coluna] = true;
            }
        }
        else
        {
            pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna);
            if (Livre(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
            if (Livre(pos) && QtdeMovimentos == 0)
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (ExisteInimigo(pos))
                matriz[pos.Linha, pos.Coluna] = true;

            // # Jogada especial - en passant
            if (Posicao.Linha == 4)
            {
                Posicao esquerda = new (Posicao.Linha, Posicao.Coluna - 1);
                if (ExisteInimigo(esquerda) && Tabuleiro.Peca(esquerda) == _partida.VulneravelEnPassant)
                    matriz[esquerda.Linha + 1, esquerda.Coluna] = true;
            
                Posicao direita = new (Posicao.Linha, Posicao.Coluna + 1);
                if (ExisteInimigo(direita) && Tabuleiro.Peca(direita) == _partida.VulneravelEnPassant)
                    matriz[direita.Linha + 1, direita.Coluna] = true;
            }
        }

        return matriz;
    }
}