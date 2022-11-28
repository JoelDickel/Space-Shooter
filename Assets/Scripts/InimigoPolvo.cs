using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoPolvo : InimigoPai
{
    //pegando o rb
    private Rigidbody2D rbPolvo;

    

  

    //pegando a posição do tiro
    [SerializeField] private Transform posicaoTiro;


   
    
    // Start is called before the first frame update
    void Start()
    {
        //pegando rb
        rbPolvo = GetComponent<Rigidbody2D>();
        //dando velocidade
        rbPolvo.velocity = new Vector2(0f, velocidade);

        //deixando espera aleatória
        esperaTiro = Random.Range(0.5f, 2f);

    }

    // Update is called once per frame
    void Update()
    {
        //checando se o sprite render está visível

        //pegando informações dos filhos
        Atirano();

    }

    private void Atirano()
    {
        bool visivel = GetComponentInChildren<SpriteRenderer>().isVisible;
        
        if (visivel)
        {
            //diminuir a espera
            esperaTiro -= Time.deltaTime;
            if (esperaTiro <= 0)
            {

               var tiro =  Instantiate(meuTiro, posicaoTiro.position, transform.rotation);
                tiro.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocidadeTiro;
                esperaTiro = Random.Range(1.5f, 2f);
                TocaTiro();
            }
        }
    }



}
