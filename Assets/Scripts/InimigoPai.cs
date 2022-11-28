using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class InimigoPai : MonoBehaviour
{
    // Start is called before the first frame update
    //atributos que todos os inimigos deve ter
    [SerializeField] protected float velocidade;
    [SerializeField] protected int vida;
    [SerializeField] protected float velocidadeTiro = 5f;
    [SerializeField] protected GameObject explosao;
    [SerializeField] protected GameObject meuTiro;
    [SerializeField] protected int pontos;
    [SerializeField] protected float esperaTiro = 1f;
    [SerializeField] protected GameObject powerUp;
    [SerializeField] protected float itemRate = 0.9f;
    [SerializeField] protected AudioClip somTiro;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void TocaTiro()
    {
        AudioSource.PlayClipAtPoint(somTiro, Vector3.zero);
    }

    //criando o metodo perde vida
    public void PerdeVida(int dano)
    {
        //so vai funcionar se o y for menor que 5
        if (transform.position.y < 5f)
        {
            vida -= dano;

            if (vida <= 0)
            {
                Destroy(gameObject);

                Instantiate(explosao, transform.position, transform.rotation);

                //criando o dropa vida
                if (powerUp)
                {
                    DropaItens();
                }

                //ganhando pontos após morrer
                var gerador = FindObjectOfType<GeradorInimigo>();
                //gerador.DiminuindoQunatidade();
                if (gerador) 
                { 
                    gerador.GanhaPontos(pontos); 
                }
               
            }
        }
    }

    //evento de quando eu me destruo
    private void OnDestroy()
    {
        var gerador = FindObjectOfType<GeradorInimigo>();
        //só executar se o código abaixo existir
        if (gerador)
        {
            gerador.DiminuindoQunatidade();
        }
    }

    //se destruindo ao colidir com o destruidor
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checando se o outro cara é destruidor
        if (other.CompareTag("Destruidor"))
        {
            Destroy(gameObject);
            //criando a explosao
            Instantiate(explosao, transform.position, transform.rotation);
           // var gerador = FindObjectOfType<GeradorInimigo>();
            //gerador.DiminuindoQunatidade();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Jogador"))
        {
            Destroy(gameObject);
           // var gerador = FindObjectOfType<GeradorInimigo>();
            //gerador.DiminuindoQunatidade();
            Instantiate(explosao, transform.position, transform.rotation);

            //tirando vida do player
            other.gameObject.GetComponent<PlayerController>().PerdeVidaPlayer(1);

            DropaItens();
        }
    }

    public void DropaItens()
    {
        //calculando a chnece de dropar o item
        float chance = Random.Range(0f, 1f);

        if (chance > itemRate) //10% de chance de dropar o item
        {
            //criando o power up e identificando ele
            GameObject pU = Instantiate(powerUp, transform.position, transform.rotation);


            //fazendo ele se mover
            //achando o rb e dando a velocidade para ele
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            pU.GetComponent<Rigidbody2D>().velocity = dir;

            //mandando o pU ser destruido em 3s
            Destroy(pU, 3f);
        }
    }
}
