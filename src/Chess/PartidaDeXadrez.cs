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
    public bool Xeque { get; private set; }
    public Peca? VulneravelEnPassant { get; private set; }
    private readonly HashSet<Peca> Pecas;
    private readonly HashSet<Peca> Capturadas;

    public PartidaDeXadrez()
    {
        Tabuleiro = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branca;
        Encerrada = false;
        Xeque = false;
        Pecas = new HashSet<Peca>();
        Capturadas = new HashSet<Peca>();
        SetupTabuleiro();
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
        if (Tabuleiro.Peca(origem).MovimentoPossivel(destino) is false)
            throw new TabuleiroException("Posição de destino inválida!");
    }

    public HashSet<Peca> PecasCapturadas(Cor cor)
    {
        var aux = new HashSet<Peca>();

        foreach (var c in Capturadas)
            if (c.Cor == cor) aux.Add(c);

        return aux;
    }

    public HashSet<Peca> PecasEmJogo(Cor cor)
    {
        var aux = new HashSet<Peca>();

        foreach (var p in Pecas)
            if (p.Cor == cor) aux.Add(p);

        aux.ExceptWith(PecasCapturadas(cor));

        return aux;
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

        Peca pecaMovida = Tabuleiro.Peca(destino);

        // # Jogada especial - promoção
        if (pecaMovida is Peao)
        {
            if (pecaMovida.Cor == Cor.Branca && destino.Linha == 0
                || pecaMovida.Cor == Cor.Preta && destino.Linha == 7)
            {
                pecaMovida = Tabuleiro.RetirarPeca(destino)!;
                Pecas.Remove(pecaMovida);
                Dama dama = new (Tabuleiro, pecaMovida.Cor);
                Tabuleiro.ColocarPeca(dama, destino);
                Pecas.Add(dama);
            }
        }

        if (EstaEmXeque(Adversaria(JogadorAtual)))
            Xeque = true;
        else
            Xeque = false;

        if (TesteXequeMate(Adversaria(JogadorAtual)))
            Encerrada = true;
        else
        {
            Turno++;
            MudarJogador();
        }

        // # Jogada especial - en passant
        if (pecaMovida is Peao
            && (destino.Linha == origem.Linha - 2
                || destino.Linha == origem.Linha + 2)) // se é um Peão e andou 2 casas a mais ou a menos
        {
            VulneravelEnPassant = pecaMovida;
        }
        else
        {
            VulneravelEnPassant = null;
        }
    }

    Peca? ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca? p = Tabuleiro.RetirarPeca(origem);
        p!.IncrementarQtdeMovimentos();
        Peca? pecaCapturada = Tabuleiro.RetirarPeca(destino);
        Tabuleiro.ColocarPeca(p, destino);

        if (pecaCapturada is not null)
            Capturadas.Add(pecaCapturada);

        // # Jogada especial - roque pequeno
        if (p is Rei && destino.Coluna == origem.Coluna + 2)
        {
            Posicao origemT = new(origem.Linha, origem.Coluna + 3);
            Posicao destinoT = new(origem.Linha, origem.Coluna + 1);
            Peca? T = Tabuleiro.RetirarPeca(origemT);
            T!.IncrementarQtdeMovimentos();
            Tabuleiro.ColocarPeca(T, destinoT);
        }

        // # Jogada especial - roque grande
        if (p is Rei && destino.Coluna == origem.Coluna - 2)
        {
            Posicao origemT = new(origem.Linha, origem.Coluna - 4);
            Posicao destinoT = new(origem.Linha, origem.Coluna - 1);
            Peca? T = Tabuleiro.RetirarPeca(origemT);
            T!.IncrementarQtdeMovimentos();
            Tabuleiro.ColocarPeca(T, destinoT);
        }

        // # Jogada especial - en passant
        if (p is Peao)
        {
            if (origem.Coluna != destino.Coluna && pecaCapturada is null)
            {
                Posicao posP;

                if (p.Cor == Cor.Branca)
                    posP = new(destino.Linha + 1, destino.Coluna);
                else
                    posP = new(destino.Linha - 1, destino.Coluna);

                pecaCapturada = Tabuleiro.RetirarPeca(posP);
                Capturadas.Add(pecaCapturada!);
            }
        }

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

        // # Jogada especial - roque pequeno
        if (p is Rei && destino.Coluna == origem.Coluna + 2)
        {
            Posicao origemT = new(origem.Linha, origem.Coluna + 3);
            Posicao destinoT = new(origem.Linha, origem.Coluna + 1);
            Peca? T = Tabuleiro.RetirarPeca(destinoT);
            T!.DecrementarQtdeMovimentos();
            Tabuleiro.ColocarPeca(T, origemT);
        }

        // # Jogada especial - roque grande
        if (p is Rei && destino.Coluna == origem.Coluna - 2)
        {
            Posicao origemT = new(origem.Linha, origem.Coluna - 4);
            Posicao destinoT = new(origem.Linha, origem.Coluna - 1);
            Peca? T = Tabuleiro.RetirarPeca(destinoT);
            T!.DecrementarQtdeMovimentos();
            Tabuleiro.ColocarPeca(T, origemT);
        }

        // # Jogada especial - en passant
        if (p is Peao)
        {
            if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
            {
                Peca peao = Tabuleiro.RetirarPeca(destino)!;
                Posicao posP;

                if (p.Cor == Cor.Branca)
                    posP = new(3, destino.Coluna);
                else
                    posP = new(4, destino.Coluna);

                Tabuleiro.ColocarPeca(peao, posP);
            }
        }
    }

    void MudarJogador()
    {
        if (JogadorAtual == Cor.Branca) JogadorAtual = Cor.Preta;
        else JogadorAtual = Cor.Branca;
    }

    static Cor Adversaria(Cor cor)
        => (cor == Cor.Branca) ? Cor.Preta : Cor.Branca;

    Peca? Rei(Cor cor)
    {
        foreach (var p in PecasEmJogo(cor))
            if (p is Rei) return p;

        return null;
    }

    bool EstaEmXeque(Cor cor)
    {
        Peca? R = Rei(cor) ?? throw new TabuleiroException($"Não há rei da cor {cor} no tabuleiro!");

        foreach (var peca in PecasEmJogo(Adversaria(cor)))
        {
            bool[,] matriz = peca.MovimentosPossiveis();
            // se alguma peça do tabuleiro pode mover para o rei, está em xeque
            if (matriz[R.Posicao!.Linha, R.Posicao.Coluna])
                return true;
        }

        return false;
    }

    bool TesteXequeMate(Cor cor)
    {
        if (EstaEmXeque(cor) is false) return false;

        foreach (var p in PecasEmJogo(cor))
        {
            bool[,] matriz = p.MovimentosPossiveis();

            // movimentos de tentativa para sair do xeque
            for (int i = 0; i < Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < Tabuleiro.Colunas; j++)
                {
                    if (matriz[i, j])
                    {
                        Posicao origem = p.Posicao!;
                        Posicao destino = new (i, j);

                        Peca? pecaCapturada = ExecutaMovimento(origem, destino);
                        bool testeXeque = EstaEmXeque(cor);
                        DesfazerMovimento(origem, destino, pecaCapturada);

                        if (testeXeque is false)
                            return false;
                    }
                }
            }
        }

        // executou todos os movimentos e não conseguiu sair do xeque: RIP
        return true;
    }

    void PosicionarPecaNoTabuleiro(char coluna, int linha, Peca peca)
    {
        Tabuleiro.ColocarPeca(peca,
            new PosicaoXadrez(coluna, linha).FromPosicaoXadrezToPosicaoProgram());
        Pecas.Add(peca);
    }

    void SetupTabuleiro()
    {
        // Brancas
        PosicionarPecaNoTabuleiro('a', 1, new Torre(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('c', 1, new Bispo(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('d', 1, new Dama(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('e', 1, new Rei(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('f', 1, new Bispo(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('h', 1, new Torre(Tabuleiro, Cor.Branca));
        PosicionarPecaNoTabuleiro('a', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('b', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('c', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('d', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('e', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('f', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('g', 2, new Peao(Tabuleiro, Cor.Branca, this));
        PosicionarPecaNoTabuleiro('h', 2, new Peao(Tabuleiro, Cor.Branca, this));

        // Pretas
        PosicionarPecaNoTabuleiro('a', 8, new Torre(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('c', 8, new Bispo(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('d', 8, new Dama(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('e', 8, new Rei(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('f', 8, new Bispo(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('h', 8, new Torre(Tabuleiro, Cor.Preta));
        PosicionarPecaNoTabuleiro('a', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('b', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('c', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('d', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('e', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('f', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('g', 7, new Peao(Tabuleiro, Cor.Preta, this));
        PosicionarPecaNoTabuleiro('h', 7, new Peao(Tabuleiro, Cor.Preta, this));
    }
}