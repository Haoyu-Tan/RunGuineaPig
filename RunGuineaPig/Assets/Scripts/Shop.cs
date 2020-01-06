using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField] private GameObject toBuild;
    [SerializeField] private Texture2D cursorTexture;
    
    private BuildManager builder;
    private CursorSetting cur;
    
    private bool available;
    private bool noSufficient;
    
    private int building;
    private int origin;
    private int twoD;
    private int table;
    private int stick;

    public int numbers;
    public int price;


    private bool sufficientMoney;
    private int latestMoney;


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
        table = Global.Table;
        stick = Global.Stick;
        builder = BuildManager.instance;
        Debug.Log(CursorSetting.cInstance);
        cur = CursorSetting.cInstance;
        anim = gameObject.GetComponent<Animator>();
        CheckMoney();
    }


    // public?????
    private void CheckMoney(){
        latestMoney = player.money;
        if(price <= player.money) sufficientMoney = true;
        else  sufficientMoney = false;
        anim.SetBool("Sufficiency", sufficientMoney);
    }

    private void ChangeCursorTexture(){
        cur.SetCur(cursorTexture);
    }

    private void changeBuilding(){
        builder.SetToBuild(toBuild);
    }
    // Update is called once per frame
    void Update()
    {
        if(latestMoney != player.money){
            CheckMoney();
       
        }
       // if(!sufficientMoney) return;
        
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
            Global.ShopUsing = this;
            //var anim = gameObject.GetComponent<Animator>();
            anim.SetBool("Disabling", true);
            this.changeBuilding();
        }
    }

    
    public void BuildingTable(){
        if(!Global.Start) return;
        if(Global.Condition == origin){
            // building 3D objects
            if(!sufficientMoney) return;
            Global.Condition = table;
            Global.ShopUsing = this;
            //var anim = gameObject.GetComponent<Animator>();
            anim.SetBool("Disabling", true);
            this.changeBuilding();
        }
    }

    public void Using2D (){

    }

    // This method allow the use of the fire extinguisher after the mouse click
    public void UsigFireExtin(){
        if(!Global.Start) return;
        //if(Global.Condition == origin && sufficientMoney){
        if(Global.Condition == origin){
            // we need to check the money and then....
            // reject if money is not sufficient
            if(!sufficientMoney) return;
            // here we changed the global condition and animation
            ChangeCursorTexture();

            Global.Condition = twoD;
            //Global.ConditionChanged = true;
            
            Global.ShopUsing = this;
            anim.SetBool("Disabling", true);
        }
    }

    public void UsingStick(){
        if(!Global.Start) return;
        if(Global.Condition == origin){
            // we need to check the money and then....
            // reject if money is not sufficient
            if(!sufficientMoney) return;
            // here we changed the global condition and animation
            ChangeCursorTexture();

            Global.Condition = stick;
            //Global.ConditionChanged = true;
            
            Global.ShopUsing = this;
            anim.SetBool("Disabling", true);
        }
    }

    public void StopUsing(){
        if(Global.Condition == origin)  Global.ShopUsing = null;
       // Global.Condition = origin;
        if(Global.builtNumber != 0) player.money = player.money - Global.builtNumber * price;
        Global.builtNumber = 0;
        anim.SetBool("Disabling", false);
        CheckMoney();
    }




}
