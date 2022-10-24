using System.Text.RegularExpressions;
using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;
using Xadrez_Console.Board.Exceptions;
using Xadrez_Console.Chess;
using static System.Console;

namespace Xadrez_Console.Presentation;

public static partial class Tela
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
        ConsoleColor fundoOriginal = BackgroundColor;
        ConsoleColor corFonteOriginal = ForegroundColor;
        ConsoleColor corFonteAlterada = ConsoleColor.Magenta;

        for (int i = 0; i < tabuleiro.Linhas; i++)
        {
            ForegroundColor = corFonteAlterada;
            Write($"{8 - i} ");
            ForegroundColor = corFonteOriginal;
            for (int j = 0; j < tabuleiro.Colunas; j++)
            {
                ImprimirPeca(tabuleiro.Peca(i, j));
            }
            WriteLine();
        }

        ForegroundColor = corFonteAlterada;
        WriteLine("  A B C D E F G H\n");
        BackgroundColor = fundoOriginal;
        ForegroundColor = corFonteOriginal;
    }

    public static void ImprimirTabuleiro(Tabuleiro tabuleiro, bool[,] posicoesPossiveis)
    {
        ConsoleColor fundoOriginal = BackgroundColor;
        ConsoleColor corFonteOriginal = ForegroundColor;
        ConsoleColor corFonteAlterada = ConsoleColor.Magenta;

        for (int i = 0; i < tabuleiro.Linhas; i++)
        {
            ForegroundColor = corFonteAlterada;
            Write($"{8 - i} ");
            ForegroundColor = corFonteOriginal;
            for (int j = 0; j < tabuleiro.Colunas; j++)
            {
                if (posicoesPossiveis[i, j]) BackgroundColor = ConsoleColor.DarkGray;
                else BackgroundColor = fundoOriginal;

                ImprimirPeca(tabuleiro.Peca(i, j));
                BackgroundColor = fundoOriginal;
            }
            WriteLine();
        }

        ForegroundColor = corFonteAlterada;
        WriteLine("  A B C D E F G H\n");
        BackgroundColor = fundoOriginal;
        ForegroundColor = corFonteOriginal;
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
        string input = ReadLine()!;
        ValidarInput(input);

        int linha = int.Parse(input[1].ToString());
        char coluna = input.ToLower()[0];
        return new(coluna, linha);
    }

    private static void ValidarInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new TabuleiroException("Você deve informar uma posição!");

        if (PosicaoRegex().IsMatch(input) is false)
            throw new TabuleiroException("Posição inválida.");
    }

    [GeneratedRegex("^[a-h][1-8]$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.NonBacktracking)]
    private static partial Regex PosicaoRegex();
}