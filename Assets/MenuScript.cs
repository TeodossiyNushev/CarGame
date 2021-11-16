using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour   
{
    public GameObject LoadingScreen;
    public GameObject StartScene;


    // Start is called before the first frame update
    public void StartGame()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        LoadingScreen.SetActive(true);
        StartScene.SetActive(false);
        if (loadingOperation.isDone)
        {
            LoadingScreen.SetActive(false);
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitGameEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void RateGame()
    {
        Application.OpenURL("https://play.google.com/store/apps");
    }
}
