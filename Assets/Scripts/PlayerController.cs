using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{

    private Rigidbody2D rb;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] GameObject tiroPlayer;
    [SerializeField] GameObject tiroPlayer2;

    [SerializeField] private Transform localTiroPlayer;

    [SerializeField] private int vida = 3;

    [SerializeField] private GameObject explosao;

    //velocidade do tiro
    [SerializeField] private float velocidadeTiro = 10f;

    [SerializeField] private float xLimite;
    [SerializeField] private float yLimite;

    [SerializeField] private int levelTiro = 1;

    [SerializeField] GameObject powerUp;
    private GameObject escudoAtual;
    private float escudoTime = 0f;
    [SerializeField] private int qtdEscudo = 3;


    [SerializeField] private Text vidaTexto;
    [SerializeField] private Text escudoTexto;


    [SerializeField] private AudioClip somTiro;

    [SerializeField] private GameObject meuEscudo;
    [SerializeField] private AudioClip somEscudo;
    [SerializeField] private AudioClip somEscudoDown;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaTexto.text = vida.ToString();
        escudoTexto.text = qtdEscudo.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        Movendo();

        Atirando();

        CriaEscudo();
    }

    private void CriaEscudo()
    {

        
        //criando o escudo
        if (Input.GetButtonDown("Shield") && qtdEscudo > 0)
        {
            //instanciando o escudo
            if (!escudoAtual)
            {
                escudoAtual = Instantiate(meuEscudo, transform.position, transform.rotation);

                //diminuindo a qtd
                qtdEscudo--;
                escudoTexto.text = qtdEscudo.ToString();
                AudioSource.PlayClipAtPoint(somEscudo, Vector3.zero);
            }
        }

        //fazendo o escudo atual seguir o player se escudo atual existe
        if (escudoAtual)
        {

            escudoAtual.transform.position = transform.position;
            //se eu tenho escudo conto o tempo
            escudoTime += Time.deltaTime;

            //se ja passaram 6 seg entao destroi escudo
            if (escudoTime > 6.2f)
            {
                Destroy(escudoAtual);

                //zerar timer
                escudoTime = 0f;

                AudioSource.PlayClipAtPoint(somEscudoDown, Vector3.zero);
            }


        }
    }

    private void Movendo()
    {
        //pegando o input vertical
        float vertical = Input.GetAxis("Vertical");
        //pegando o input horizontal
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 minhaVelocidade = new Vector2(horizontal, vertical);
        //normalizando a velocidade
        minhaVelocidade.Normalize();
        //passando a minha velocidade para o meu rb
        rb.velocity = minhaVelocidade * velocidade;


        //limitando a posição dele na tela
        //Clamp**
        //checando posição x
        float meuX = Mathf.Clamp(transform.position.x, -xLimite, xLimite);
        float meuY = Mathf.Clamp(transform.position.y, -yLimite, yLimite);

        //palicando meux e meuy à minha posição
        transform.position = new Vector3(meuX, meuY, transform.position.z);

    }

    private void Atirando()
    {


        //atirando com o player
        if (Input.GetButtonDown("Fire1"))
        {

            AudioSource.PlayClipAtPoint(somTiro, Vector3.zero);


            if (levelTiro == 1)
            {
                CriaTiro(tiroPlayer, localTiroPlayer.position);
            }
            if (levelTiro == 2)
            {
                //criando 2 tiros
                //tiro esquerda
                Vector3 posicaoE = new Vector3(transform.position.x - 0.45f, transform.position.y + 0.1f, 0);
                CriaTiro(tiroPlayer2, posicaoE);
                //tiro direita
                Vector3 posicaoD = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 0);
                CriaTiro(tiroPlayer2, posicaoD);
            }
            if (levelTiro == 3)
            {
                CriaTiro(tiroPlayer, localTiroPlayer.position);
                //criando 2 tiros
                //tiro esquerda
                Vector3 posicaoE = new Vector3(transform.position.x - 0.45f, transform.position.y + 0.1f, 0);
                CriaTiro(tiroPlayer2, posicaoE);
                //tiro direita
                Vector3 posicaoD = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 0);
                CriaTiro(tiroPlayer2, posicaoD);
            }
        }
    }

    private void CriaTiro(GameObject tiroCriado, Vector3 posicao)
    {
        GameObject tiro = Instantiate(tiroCriado, posicao, transform.rotation);
        //dar a direção e velocidade do tiro
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, velocidadeTiro);
    }

    public void PerdeVidaPlayer(int dano)
    {
        vida -= dano;

        vidaTexto.text = vida.ToString();

        if (vida <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosao, transform.position, transform.rotation);

            //carregando a cena inicial do jogo
            //achando o game manager
            var gameManager = FindObjectOfType<GameManager>();
            //rodando o método de iniciar o jogo SE o game manager existe
            if (gameManager)
            {
                gameManager.Inicio();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (levelTiro < 3)
            {
                levelTiro++;

            }
            Destroy(other.gameObject);
        }
    }
}

