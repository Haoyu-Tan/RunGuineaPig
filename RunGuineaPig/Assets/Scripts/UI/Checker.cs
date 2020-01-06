using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject box;
    public GameObject boxCross;
    
    
    void Start()
    {
        checkActive();
    }

    private void checkActive(){
        if(GlobalSettings.Guide){
            boxCross.SetActive(true);
            box.SetActive(false);
        }
        else{
            boxCross.SetActive(false);
            box.SetActive(true);
        }
    }

    public void ChangeGuideStatus(){
        GlobalSettings.Guide = !GlobalSettings.Guide;
        checkActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
