using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;

namespace Xadrez_Console.Chess;

public class PartidaDeXadrez
{
    public Tabuleiro Tabuleiro { get; private set; }
    public int Turno { get; private set; }
    public Cor JogadorAtual { get; private set; }
    public bool Encerrada { get; private set; }

    public PartidaDeXadrez()
    {
        Tabuleiro = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branca;
        Encerrada = false;
        ColocarPecas();
    }

    void ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca? p = Tabuleiro.RetirarPeca(origem);
        p!.IncrementarQtdeMovimentos();
        Peca? pecaCapturada = Tabuleiro.RetirarPeca(destino);
        Tabuleiro.ColocarPeca(p, destino);
    }

    public void RealizarJogada(Posicao origem, Posicao destino)
    {
        ExecutaMovimento(origem, destino);
        Turno++;
        MudarJogador();
    }

    void MudarJogador()
    {
        if (JogadorAtual == Cor.Branca) JogadorAtual = Cor.Preta;
        else JogadorAtual = Cor.Branca;
    }

    void ColocarPecas()
    {
        // Brancas
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('c', 1).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('c', 2).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Rei(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('d', 1).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('d', 2).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('e', 1).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Branca),
            new PosicaoXadrez('e', 2).FromPosicaoXadrezToPosicaoProgram()
        );

        // Pretas
        Tabuleiro.ColocarPeca(
    new Torre(Tabuleiro, Cor.Preta),
    new PosicaoXadrez('c', 7).FromPosicaoXadrezToPosicaoProgram()
);
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Preta),
            new PosicaoXadrez('c', 8).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Rei(Tabuleiro, Cor.Preta),
            new PosicaoXadrez('d', 8).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Preta),
            new PosicaoXadrez('d', 7).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Preta),
            new PosicaoXadrez('e', 7).FromPosicaoXadrezToPosicaoProgram()
        );
        Tabuleiro.ColocarPeca(
            new Torre(Tabuleiro, Cor.Preta),
            new PosicaoXadrez('e', 8).FromPosicaoXadrezToPosicaoProgram()
        );
    }
}