using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroController : MonoBehaviour
{
    

    [SerializeField] private GameObject impacto;


    private Rigidbody2D rbTiroPlayer;
    void Start()
    {
        //pegando o rb
        rbTiroPlayer = GetComponent<Rigidbody2D>();

        //indo para cima
        //rbTiroPlayer.velocity = new Vector2(0f, velocidadeTiroPlayer);
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //pegar o metodo perde vida e aplicar nele o dano
        //isso só deve dar se for o polvo
        //checando se é o polvo
        if (collision.CompareTag("InimigoPolvo"))
        {
            var ini = collision.GetComponent<InimigoPai>();
            ini.PerdeVida(1);

          
        }
        //checando se é o jogador
        if (collision.CompareTag("Jogador"))
        {
            collision.GetComponent<PlayerController>().PerdeVidaPlayer(1);
        }

        Destroy(gameObject);

        //criando efeito impacto
        Instantiate(impacto, transform.position, transform.rotation);
    }
}
