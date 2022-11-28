


using UnityEngine;
using UnityEngine.UI;

public class BossController : InimigoPai
{
    // Start is called before the first frame update
    [Header("Informações básicas")]
     private string estado = "estado1";

     private Rigidbody2D rb;
    [SerializeField] private float limiteHorizontal = 6f;

    [SerializeField] private bool direita = true;

    [Header("Informações dos tiros")]
    [SerializeField] private Transform posicaoTiro1e;
    [SerializeField] private Transform posicaoTiro1d;
    [SerializeField] private Transform posicaoTiro2;
    [SerializeField] private GameObject tiro1;
    [SerializeField] private GameObject tiro2;
     private float delayTiro = 1f;
     private float esperaTiro2 = 1;
    [SerializeField] private string[] estados;
    private float esperaEstado = 10f;

    [SerializeField] private Image vidaImagem;
    [SerializeField] private int maximaVida = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Dando a minha vida inicial
        vida = maximaVida;


        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()


    {

        TrocaEstado();
        switch (estado)
        {
            case "estado1":
                Estado1();
                break;
            case "estado2":
                Estado2();
                break;
                case "estado3":
                Estado3();
                    break;

        }

        //atualizando a vida do boss
        //garantir que a divisão vai retornar um float
        vidaImagem.fillAmount = ((float) vida / (float)maximaVida);
        //converter  o valor do fillamount para alguma coisa entre 0 e 255

        //convertendo o valor do fillamoun e garntindo que o tipo é Byte
        vidaImagem.color = new Color32(190, (byte) (vidaImagem.fillAmount *255), 54, 255);
    }

    private void AumentaDificuldade()
    {
        //checando se minha vida é menor ou igual a metade da vida
        if(vida <= maximaVida / 2)
        {
            delayTiro = 0.8f;
        }
    }

    private void Estado3()
    {

        //deixando parado
        rb.velocity = Vector2.zero;
        if (esperaTiro <= 0f)
        {
            Tiro1();
            esperaTiro = delayTiro;
        }
        else
        {
            esperaTiro -= Time.deltaTime;
        }

        if (esperaTiro2 <= 0f)
        {
            Tiro2();
            esperaTiro2 = delayTiro;
        }
        else
        {
            esperaTiro2 -= Time.deltaTime;
        }
    }

    private void Estado2()
    {
        rb.velocity = Vector2.zero;
        if (esperaTiro <= 0f)
        {
            Tiro2();
            esperaTiro = delayTiro/2;
        }
        else
        {
            esperaTiro -= Time.deltaTime;
        }
    }

    private void Estado1()
    {
        if (esperaTiro <= 0f)
        {
            Tiro1();
            esperaTiro = delayTiro;
        }
        else
        {
            esperaTiro -= Time.deltaTime;
        }

        //indo para a direota e para a esquerda
        if (direita)
        {
            rb.velocity = new Vector2(velocidade, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-velocidade, 0f);
        }

        //mudando valor de direira
        if (transform.position.x >= limiteHorizontal)
        {
            direita = false;
        }
        else if (transform.position.x <= -limiteHorizontal)
        {
            direita = true;
        }


    }

    private void Tiro1()
    {
        TocaTiro();
        //criando tiro
        GameObject tiro = Instantiate(tiro1, posicaoTiro1e.position, transform.rotation);
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -velocidadeTiro);
        tiro = Instantiate(tiro1, posicaoTiro1d.position, transform.rotation);
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -velocidadeTiro);
    }

    private void Tiro2()
    {
        TocaTiro();
        var player = FindObjectOfType<PlayerController>();
        //encontrando o valor da direção
        if (player)
        {


            var tiro = Instantiate(tiro2, posicaoTiro2.position, transform.rotation);
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






        }
    }

    private void TrocaEstado()
    {
        if(esperaEstado <= 0f)
        {
            // mudando estados 
            //escohler um valor aleatório
            int indiceEstado = Random.Range(0, estados.Length);
            estado = estados[indiceEstado];

            esperaEstado = 10f;
        }
        else
        {
            esperaEstado -= Time.deltaTime;
        }
    }
}
