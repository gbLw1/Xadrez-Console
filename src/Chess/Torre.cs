using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class Torre : Peca
{
    public Torre(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) { }

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
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != this.Cor)
                break;
            pos.Linha -= 1;
        }

        // baixo
        pos.DefinirValores(Posicao!.Linha + 1, Posicao.Coluna);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != this.Cor)
                break;
            pos.Linha += 1;
        }

        // direita
        pos.DefinirValores(Posicao!.Linha, Posicao.Coluna + 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != this.Cor)
                break;
            pos.Coluna += 1;
        }

        // esquerda
        pos.DefinirValores(Posicao!.Linha, Posicao.Coluna - 1);
        while (PodeMover(pos))
        {
            matriz[pos.Linha, pos.Coluna] = true;
            if (Tabuleiro.Peca(pos) is not null && Tabuleiro.Peca(pos).Cor != this.Cor)
                break;
            pos.Coluna -= 1;
        }

        return matriz;
    }

    public override string ToString() => "T";
}