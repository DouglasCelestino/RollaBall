using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Menu_iniciar()
    {
        SceneManager.LoadScene(0);
    }

    public void Jogo()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
