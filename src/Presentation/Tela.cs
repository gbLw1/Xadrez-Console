using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;
using Xadrez_Console.Chess;
using static System.Console;

namespace Xadrez_Console.Presentation;

public static class Tela
{
    public static void ImprimirPartida(PartidaDeXadrez partida)
    {
        ImprimirTabuleiro(partida.Tabuleiro);
        ImprimirPecasCapturadas(partida);
        WriteLine($"Turno: {partida.Turno}");
        if (partida.Encerrada is false)
        {
            WriteLine($"Aguardando jogada: {partida.JogadorAtual}");
            if (partida.Xeque) WriteLine("XEQUE!");
        }
        else
        {
            WriteLine("XEQUEMATE!");
            WriteLine($"Vencedor: {partida.JogadorAtual}");
        }
    }

    static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
    {
        ConsoleColor aux = ForegroundColor;

        WriteLine("Peças capturadas:");
        Write("Brancas: ");
        ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
        WriteLine();
        Write("Pretas: ");
        ForegroundColor = ConsoleColor.Yellow;
        ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
        ForegroundColor = aux;
        WriteLine();
    }

    static void ImprimirConjunto(HashSet<Peca> pecas)
    {
        Write("[ ");

        foreach (var p in pecas)
            Write($"{p} ");

        Write("]");
    }

    public static void ImprimirTabuleiro(Tabuleiro tabuleiro)
    {
        for (int i = 0; i < tabuleiro.Linhas; i++)
        {
            Write($"{8 - i} ");
            for (int j = 0; j < tabuleiro.Colunas; j++)
            {
                ImprimirPeca(tabuleiro.Peca(i, j));
            }
            WriteLine();
        }
        WriteLine("  A B C D E F G H\n");
    }

    public static void ImprimirTabuleiro(Tabuleiro tabuleiro, bool[,] posicoesPossiveis)
    {
        ConsoleColor fundoOriginal = BackgroundColor;
        ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

        for (int i = 0; i < tabuleiro.Linhas; i++)
        {
            Write($"{8 - i} ");
            for (int j = 0; j < tabuleiro.Colunas; j++)
            {
                if (posicoesPossiveis[i, j]) BackgroundColor = fundoAlterado;
                else BackgroundColor = fundoOriginal;

                ImprimirPeca(tabuleiro.Peca(i, j));
                BackgroundColor = fundoOriginal;
            }
            WriteLine();
        }
        WriteLine("  A B C D E F G H\n");
        BackgroundColor = fundoOriginal;
    }

    public static void ImprimirPeca(Peca peca)
    {
        if (peca is null)
            Write("- ");
        else
        {
            if (peca.Cor == Cor.Branca)
                Write(peca);
            else
            {
                ConsoleColor aux = ForegroundColor;
                ForegroundColor = ConsoleColor.Yellow;
                Write(peca);
                ForegroundColor = aux;
            }
            Write(" ");
        }
    }

    public static PosicaoXadrez LerPosicaoXadrez()
    {
        string? s = ReadLine();
        int linha = int.Parse(s![1].ToString());
        char coluna = s[0];
        return new(coluna, linha);
    }
}