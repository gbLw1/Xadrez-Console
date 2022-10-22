using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;
using Xadrez_Console.Board.Exceptions;
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
        string? s = ReadLine();
        ValidarInput(s);
        int linha = int.Parse(s![1].ToString());
        char coluna = s[0];
        return new(coluna, linha);
    }

    static void ValidarInput(string? s)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Length != 2)
            throw new TabuleiroException("Não foi possível converter a posição digitada em uma posição do tabuleiro.");

        char coluna = s[0];
        if (!int.TryParse(s[1].ToString(), out int linha)
            || coluna is not 'a' or 'b' or 'c' or 'd' or 'e' or 'f' or 'g' or 'h')
            throw new TabuleiroException("Valor inválido para Linha ou Coluna.");
    }
}