using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCanvas, optionsMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
    }

    public void NewGame(){
        //Put class system choice in here later
        SceneManager.LoadScene("Game");
    }

    public void LoadGame(){
        //Load game system will go in here.
    }

    public void OptionsButton(){
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    public void backToMainMenu(){
        mainMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
