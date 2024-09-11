using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string menu = "menu";
    [SerializeField] private string jogo = "Game";
    // Start is called before the first frame update
    public void Menu_iniciar()
    {
        SceneManager.LoadScene(menu);
    }

    public void Jogo()
    {
        SceneManager.LoadScene(jogo);
    }
    // Update is called once per frame
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
