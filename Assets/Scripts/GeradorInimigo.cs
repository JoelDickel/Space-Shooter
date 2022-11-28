using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeradorInimigo : MonoBehaviour
{

    [SerializeField] private GameObject[] inimigos;

    [SerializeField] private int pontos = 0;
    [SerializeField] private int level = 1;
    [SerializeField] private int baseLevel = 100;

    private float esperaInimigos = 0f;
    [SerializeField] private float tempoEspera = 5f;

    [SerializeField] private int qtdInimigos = 0;

    [SerializeField] private GameObject bossAnimation;
    private bool animacaoBoss = false;

    [SerializeField] private AudioClip musicaBoss;
    [SerializeField] private AudioSource musicaJogo;


    [SerializeField] private Text pontosTexto;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //após chegar nível 10, parar de chamar bichos
        //criando a animação do boss
        if (level < 10)
        {
            GeraInimigos();
        }
        else
        {
            GeraBoss();
        }
        pontosTexto.text = pontos.ToString();
    }

    private void GeraBoss()
    {
        //se a qtd inimigos é 0 e ele diminui o tempo de espera e tempo espera for maior do que 0
        if (qtdInimigos <= 0 && tempoEspera > 0)
        {
            tempoEspera -= Time.deltaTime;
        }
        //criando ó método para gerar o boss
        //instanciando a animação do boss só se ele não está ativo e tempo de espera = 0
        if (!animacaoBoss && tempoEspera <= 0)
        {
            GameObject animBoss = Instantiate(bossAnimation, Vector3.zero, transform.rotation);
          
            
            //avisando que a animacao já esta ok
            animacaoBoss = true;

            //parando a música antiga e gerando uma nova
            musicaJogo.clip = musicaBoss;
            musicaJogo.Play();
        }


    }

    //ganhando pontos
    public void GanhaPontos(int pontos)
    {
        this.pontos += pontos * level;
        

        //ganhando level se os pontos forem mais que a base do level * level
        if (this.pontos > baseLevel)
        {
            level++;

            //dobrando a quantidade de pontos necessária
            baseLevel *= 2;
        }
    }

    //diminuindo a quantidade de inimigos
    public void DiminuindoQunatidade()
    {
        qtdInimigos--;
    }

    //metodo para checar se a posicao esta livre
    private bool ChecaPosicao(Vector3 posicao, Vector3 size)
    {
        //checando se na posicao tem alguem
        //estou vendo se na posicao tem algum colisor 2d
        Collider2D hit = Physics2D.OverlapBox(posicao, size, 0f);


        //debu para checar qual info esta salva no hit


        //se hit for null ele retorna false
        //se nao ele retorna true
        if (hit != null)
        {
            return true;
        }
        else
        {
            return false;
        }

        //se o hit é null nao houve colisao
        //pode criar inimigo ali
        //se o hit nao é null = houve colisao
        //nao pode criar inimigo ali

    }


    private void GeraInimigos()
    {
        //diminuindo o tempo se ele for mair do que 0
        if (esperaInimigos > 0 && qtdInimigos <= 0)
        {
            esperaInimigos -= Time.deltaTime;
        }



        //checando se a espera já zerou
        if (esperaInimigos <= 0f && qtdInimigos <= 0)
        {
            int quantidade = level * 4;
            int tentativas = 0;

            //criando inimigos de uma só vez
            while (qtdInimigos < quantidade)
            {

                //fazendo ele sair do laço de repitição se ele repetir muitas vezes
                tentativas++;
                //se ele tentou mais de 200x ele desise
                if (tentativas > 200)
                {
                    //desistir
                    break;
                }



                GameObject inimigoCriado;

                //decidindo qual inimigo vai ser criado com base no lvl
                float chance = UnityEngine.Random.Range(0f, level);
                if (chance > 2f)
                {
                    inimigoCriado = inimigos[1];
                }
                else
                {
                    inimigoCriado = inimigos[0];
                }
                //definindo a posicao
                Vector3 posicao = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(6f, 10f), 0f);

                //checando se a posição está livre(ou seja, nao tem ngm)
                bool colisao = ChecaPosicao(posicao, inimigoCriado.transform.localScale);
                //se houve colisao eu vou pular essa repeticao
                if (colisao)
                {
                    continue;
                }

                //criar os inimigos e eles nao colidirem

                //criando um inimigo
                Instantiate(inimigoCriado, posicao, transform.rotation);
                //aumentando a quantidade
                qtdInimigos++;

                //reiniciando a espera
                esperaInimigos = tempoEspera;
            }
        }
    }

}
