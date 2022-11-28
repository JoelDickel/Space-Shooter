using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        //garantindo que só existe 1 game manager por vez
        //contando quantos games manager existem na cena
        int quantidade = FindObjectsOfType<GameManager>().Length;
        if(quantidade >1)
        {
            Destroy(gameObject);
        }
        //eu não vou ser destruido quando mudar de cena
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //criando método para ir ao jogo
    public void IniciaJogo()
    {
        SceneManager.LoadScene(1);

    }

    //criando uma método que roda depois de um certo tempo
    IEnumerator PrimeiraCena()
    {
        yield return new WaitForSeconds(2f);
        //todo código daqui só vai rodar depois de 2 segundos.
        SceneManager.LoadScene(0);
    }


    public void Inicio()
    {
        //iniciando a minha corotina
        StartCoroutine(PrimeiraCena());
    }
        

    public void Sair()
    {
        //fechando o jogo
        Application.Quit();
    }
}
