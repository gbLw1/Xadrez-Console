using Xadrez_Console.Board;
using Xadrez_Console.Board.Enums;
using Xadrez_Console.Board.Exceptions;

namespace Xadrez_Console.Chess;

public class PartidaDeXadrez
{
    public Tabuleiro Tabuleiro { get; private set; }
    public int Turno { get; private set; }
    public Cor JogadorAtual { get; private set; }
    public bool Encerrada { get; private set; }
    private readonly HashSet<Peca> Pecas;
    private readonly HashSet<Peca> Capturadas;
    public bool Xeque { get; private set; }

    public PartidaDeXadrez()
    {
        Tabuleiro = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branca;
        Encerrada = false;
        Xeque = false;
        Pecas = new HashSet<Peca>();
        Capturadas = new HashSet<Peca>();
        ColocarPecas();
    }

    public void ValidarPosicaoOrigem(Posicao posicao)
    {
        if (Tabuleiro.Peca(posicao) is null)
            throw new TabuleiroException("Não existe peça na posição de origem escolhida!");

        if (JogadorAtual != Tabuleiro.Peca(posicao).Cor)
            throw new TabuleiroException("A peça de origem escolhida não é sua!");

        if (Tabuleiro.Peca(posicao).ExisteMovimentosPossiveis() is false)
            throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
    }

    public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
    {
        if (Tabuleiro.Peca(origem).PodeMoverPara(destino) is false)
            throw new TabuleiroException("Posição de destino inválida!");
    }

    Peca? ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca? p = Tabuleiro.RetirarPeca(origem);
        p!.IncrementarQtdeMovimentos();
        Peca? pecaCapturada = Tabuleiro.RetirarPeca(destino);
        Tabuleiro.ColocarPeca(p, destino);

        if (pecaCapturada is not null)
            Capturadas.Add(pecaCapturada);

        return pecaCapturada;
    }

    void DesfazerMovimento(Posicao origem, Posicao destino, Peca? pecaCapturada)
    {
        Peca? p = Tabuleiro.RetirarPeca(destino);
        p!.DecrementarQtdeMovimentos();

        if (pecaCapturada is not null)
        {
            Tabuleiro.ColocarPeca(pecaCapturada, destino);
            Capturadas.Remove(pecaCapturada);
        }

        Tabuleiro.ColocarPeca(p, origem);
    }

    public void RealizarJogada(Posicao origem, Posicao destino)
    {
        Peca? pecaCapturada = ExecutaMovimento(origem, destino);

        // Um usário não pode se colocar em Xeque
        if (EstaEmXeque(JogadorAtual))
        {
            DesfazerMovimento(origem, destino, pecaCapturada);
            throw new TabuleiroException("Você não pode se colocar em xeque!");
        }

        if (EstaEmXeque(Adversaria(JogadorAtual))) Xeque = true;
        else Xeque = false;

        Turno++;
        MudarJogador();
    }

    void MudarJogador()
    {
        if (JogadorAtual == Cor.Branca) JogadorAtual = Cor.Preta;
        else JogadorAtual = Cor.Branca;
    }

    public HashSet<Peca> PecasCapturadas(Cor cor)
    {
        var aux = new HashSet<Peca>();

        foreach (var c in Capturadas)
        {
            if (c.Cor == cor) aux.Add(c);
        }

        return aux;
    }

    public HashSet<Peca> PecasEmJogo(Cor cor)
    {
        var aux = new HashSet<Peca>();

        foreach (var p in Pecas)
        {
            if (p.Cor == cor) aux.Add(p);
        }

        aux.ExceptWith(PecasCapturadas(cor));

        return aux;
    }

    static Cor Adversaria(Cor cor)
    {
        if (cor == Cor.Branca) return Cor.Preta;
        else return Cor.Branca;
    }

    Peca? Rei(Cor cor)
    {
        foreach (var p in PecasEmJogo(cor))
        {
            if (p is Rei) return p;
        }
        return null;
    }

    public bool EstaEmXeque(Cor cor)
    {
        Peca R = Rei(cor) ?? throw new TabuleiroException($"Não há rei da cor {cor} no tabuleiro!");

        foreach (var peca in PecasEmJogo(Adversaria(cor)))
        {
            bool[,] matriz = peca.MovimentosPossiveis();
            // se alguma peça do tabuleiro pode mover para o rei, está em xeque
            if (matriz[R.Posicao.Linha, R.Posicao.Coluna])
                return true;
        }

        return false;
    }

    void ColocarNovaPeca(char coluna, int linha, Peca peca)
    {
        Tabuleiro.ColocarPeca(peca,
            new PosicaoXadrez(coluna, linha).FromPosicaoXadrezToPosicaoProgram());
        Pecas.Add(peca);
    }

    void ColocarPecas()
    {
        // Brancas
        ColocarNovaPeca('c', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('c', 2, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('d', 1, new Rei(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('d', 2, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('e', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('e', 2, new Torre(Tabuleiro, Cor.Branca));

        // Pretas
        ColocarNovaPeca('c', 7, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('c', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('d', 8, new Rei(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('d', 7, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('e', 7, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('e', 8, new Torre(Tabuleiro, Cor.Preta));
    }
}