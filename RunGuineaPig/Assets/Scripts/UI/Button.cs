using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject toBuild;
    private BuildManager builder;
    private bool available;
    
    private int building;
    private int origin;
    private int twoD;

    public bool isShop;
   

   // private bool isTwoD;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
      //  isTwoD = false;
        available = true;
        building = Global.Building;
        origin = Global.Origin;
        twoD = Global.TwoDem;

        builder = BuildManager.instance;
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Sufficiency", true);
    }

    public void changeBuilding(){
        builder.SetToBuild(toBuild);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(Global.Condition == origin){
           available = true;
           anim.SetBool("Available", available);
           return;
       }
       else{
            available = false;
            anim.SetBool("Available", available);
       }

    }

    public void Using3D (){
        if(!Global.Start) return;
        if(Global.Condition == origin){
            // building 3D objects
            Global.Condition = building;
            Global.UIUsing = this;
            //var anim = gameObject.GetComponent<Animator>();
            anim.SetBool("Disabling", true);
            this.changeBuilding();
        }
    }

    public void StopUsing(){
        Global.UIUsing = null;
        //Global.Condition = origin;
        anim.SetBool("Disabling", false);
    }
}
