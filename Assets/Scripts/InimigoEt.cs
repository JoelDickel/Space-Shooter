using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class InimigoEt : InimigoPai

    
{

    private Rigidbody2D rb;
    [SerializeField] private Transform posicaoTiro;

    [SerializeField] private float yMax = 2.5f;

    private bool possoMover = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //iniciando movimento
        rb.velocity = Vector2.up * velocidade;

       
    }

    // Update is called once per frame
    void Update()
    {
        Atirando();
        //checando se cheguei no meio da tela e posso me mover
        if(transform.position.y < yMax && possoMover)
        {
            //checando de que lado estou
            //checando se estou na esquerca
            if (transform.position.x < 0f)
            {
                //indo para a direita
                rb.velocity = new Vector2(velocidade * -1, velocidade);
                //falando que não posso mover
                possoMover = false;
            }
            else
            {
                //indo para a esquerda
                rb.velocity = new Vector2(velocidade, velocidade);
                //falando que não posso mover
                possoMover = false;
            }

            //checando se estou na direita
        }
    }

    private void Atirando()
    {
        bool visivel = GetComponentInChildren<SpriteRenderer>().isVisible;

        

        if (visivel)
        {
            var player = FindObjectOfType<PlayerController>();
            //encontrando o valor da direção
            if (player)
            {
                esperaTiro -= Time.deltaTime;
                if (esperaTiro <= 0f)
                {
                    var tiro = Instantiate(meuTiro, posicaoTiro.position, transform.rotation);
                    //encontrando o player na cena

                    Vector2 direcao = player.transform.position - tiro.transform.position;
                    //normalizando a velocidade
                    direcao.Normalize();
                    //dando a direção e velocidade do tiro
                    tiro.GetComponent<Rigidbody2D>().velocity = direcao * velocidadeTiro;

                    //dando o angulo que o tiro tem que ter
                    float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
                    //passando o angulo
                    tiro.transform.rotation = Quaternion.Euler(0f, 0f, angulo + 90f);

                    esperaTiro = Random.Range(2f, 4f);

                    TocaTiro();
                }

            }
        }
    }
}
