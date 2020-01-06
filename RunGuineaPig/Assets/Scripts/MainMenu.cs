using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
 
 
public class MainMenu : MonoBehaviour
{

    public GameObject level;
    private GameObject menu;

    void Start(){
        menu = this.gameObject;
    }

    public void PlayGame(){
       menu.SetActive(false);
       level.SetActive(true);
       
    }


    public void BackToMenu(){
        menu.SetActive(true);
       level.SetActive(false);
    }

    public void level1(){
        SceneManager.LoadScene(1);
    }

    public void level2(){
        SceneManager.LoadScene(2);
    }

    public void level3(){
        SceneManager.LoadScene(3);
    }

    public void level4(){
        SceneManager.LoadScene(4);
    }
    
    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
