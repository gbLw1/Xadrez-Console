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
    public Peca? vulneravelEnPassant { get; private set; }

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
        if (Tabuleiro.Peca(origem).MovimentoPossivel(destino) is false)
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
            if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
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

        if (TesteXequeMate(Adversaria(JogadorAtual))) Encerrada = true;
        else
        {
            Turno++;
            MudarJogador();
        }

        Peca pecaMovida = Tabuleiro.Peca(destino);

        // # Jogada especial - en passant
        if (pecaMovida is Peao
            && (destino.Linha == origem.Linha - 2
                || destino.Linha == origem.Linha + 2)) // se é um Peão e andou 2 casas a mais ou a menos
        {
            vulneravelEnPassant = pecaMovida;
        }
        else
        {
            vulneravelEnPassant = null;
        }
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

    bool EstaEmXeque(Cor cor)
    {
        Peca? R = Rei(cor);
        if (R is null) throw new TabuleiroException($"Não há rei da cor {cor} no tabuleiro!");

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
                        Posicao destino = new Posicao(i, j);

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

    void ColocarNovaPeca(char coluna, int linha, Peca peca)
    {
        Tabuleiro.ColocarPeca(peca,
            new PosicaoXadrez(coluna, linha).FromPosicaoXadrezToPosicaoProgram());
        Pecas.Add(peca);
    }

    void ColocarPecas()
    {
        // Brancas
        ColocarNovaPeca('a', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('c', 1, new Bispo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('d', 1, new Dama(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('e', 1, new Rei(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('f', 1, new Bispo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('h', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('a', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('b', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('c', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('d', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('e', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('f', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('g', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('h', 2, new Peao(Tabuleiro, Cor.Branca, this));

        // Pretas
        ColocarNovaPeca('a', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('c', 8, new Bispo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('d', 8, new Dama(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('e', 8, new Rei(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('f', 8, new Bispo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('h', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('a', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('b', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('c', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('d', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('e', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('f', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('g', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('h', 7, new Peao(Tabuleiro, Cor.Preta, this));
    }
}