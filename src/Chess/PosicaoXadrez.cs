using Xadrez_Console.Board;

namespace Xadrez_Console.Chess;

public class PosicaoXadrez
{
    public char Coluna { get; set; }
    public int Linha { get; set; }

    public PosicaoXadrez(char coluna, int linha)
    {
        Coluna = coluna;
        Linha = linha;
    }

    public static implicit operator string(PosicaoXadrez p) => p.ToString();

    public Posicao FromPosicaoXadrezToPosicaoProgram()
        => new(8 - Linha, Coluna - 'a');

    public override string ToString()
        => $"{Coluna}{Linha}";
}