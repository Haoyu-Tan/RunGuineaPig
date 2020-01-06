using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonState : MonoBehaviour
{
    private Animator anim;
    public bool highlight;
    public GameObject guide;
    // Start is called before the first frame update
    void Start()
    {
        highlight = false;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    

    public void highlighted(){
        highlight = true;
        anim.SetBool("Highlighting", true);
    }

    public void normal(){
        highlight = false;
        anim.SetBool("Highlighting", false);
    }

    public void OpenGuide(){
        if(GlobalSettings.Guide && !Global.Paused){
            guide.SetActive(true);
        }
    }

    public void CloseGuide(){
        if(GlobalSettings.Guide && !Global.Paused){
            guide.SetActive(false);
        }
    }
}
