using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void Back(){
        //Global.GS.Resume();
        Debug.Log("INDEX IS " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }
    
    public void Restart(){
        //Global.GS.Resume();
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(){
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }






    
    
}