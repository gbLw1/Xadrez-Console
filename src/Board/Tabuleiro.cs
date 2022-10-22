using Xadrez_Console.Board.Exceptions;

namespace Xadrez_Console.Board;

public class Tabuleiro
{
    public int Linhas { get; set; }
    public int Colunas { get; set; }
    private readonly Peca[,] Pecas;

    public Tabuleiro(int linhas, int colunas)
    {
        Linhas = linhas;
        Colunas = colunas;
        Pecas = new Peca[Linhas, Colunas];
    }

    public Peca Peca(int linha, int coluna)
        => Pecas[linha, coluna];

    public Peca Peca(Posicao posicao)
        => Pecas[posicao.Linha, posicao.Coluna];

    bool ExistePeca(Posicao posicao)
    {
        ValidarPosicao(posicao);
        return Peca(posicao) is not null;
    }

    public void ColocarPeca(Peca peca, Posicao posicao)
    {
        if (ExistePeca(posicao)) throw new TabuleiroException("Já existe uma peça nessa posição!");

        // Matriz de peças recebe a peça nas coordenadas pos.X & pos.Y
        Pecas[posicao.Linha, posicao.Coluna] = peca;
        peca.Posicao = posicao;
    }

    public Peca? RetirarPeca(Posicao posicao)
    {
        if (ExistePeca(posicao) is false) return null;

        Peca aux = Peca(posicao);
        aux.Posicao = null;
        Pecas[posicao.Linha, posicao.Coluna] = null!;
        return aux;
    }

    public bool PosicaoValida(Posicao posicao)
        => !(posicao.Linha < 0 || posicao.Linha >= Linhas || posicao.Coluna < 0 || posicao.Coluna >= Colunas);

    public void ValidarPosicao(Posicao posicao)
    {
        if (PosicaoValida(posicao) is false) throw new TabuleiroException("Posição inválida!");
    }
}