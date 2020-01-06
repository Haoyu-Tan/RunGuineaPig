using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public float Adore;
    public float moneyPerFrame;

    private float time0;
    private float ExtinTime;
    private float deltaTime;
    private float moneyTime;

    private Vector3 originalScale;

    private GameObject realFire;
    private GameObject indicateFire;
    private GameObject activating;
    private GameObject ParentNode;


    private bool mouseOnFire;
    private bool start;
    
    
    private int currentState;
    private int origin;
    private int fireExtin;


    
    // Start is called before the first frame update
    void Start()
    {
        currentState = Global.Condition;
        origin = Global.Origin;
        fireExtin = Global.TwoDem;
        time0 = 0f;
        realFire = this.gameObject.transform.GetChild(0).gameObject;
        indicateFire = this.gameObject.transform.GetChild(1).gameObject;
        originalScale = realFire.transform.localScale;
        activating = realFire;
        //mouseOn = false;
        start = false;
        mouseOnFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ExtinTime >= Adore) {
            FireExtined();
            Debug.Log("fire off!!!!!!!!!!!!!");
            return;
        }
        if(currentState != Global.Condition){
            if(Global.Condition != fireExtin) mouseOnFire = false;
            currentState = Global.Condition;
            
            
        }
        // 
        
        if(mouseOnFire){
            IndicateOn();
            if(player.money <= 0){
                Global.Condition = origin;
                start = false;
            }
            if(start){
                deltaTime = Time.time - time0;
                Debug.Log(deltaTime);
                // money used
                // needs to change sth. cost is too high
                moneyTime = moneyTime + deltaTime;
                if(moneyTime >= 0.1f){
                    player.money--;
                    if(player.money == 0) Global.Condition = origin;
                    moneyTime = moneyTime - 0.1f;
                }
                ExtinTime = ExtinTime + deltaTime;
                //Debug.Log(ExtinTime);

                // here fire size changed

                ChangeScale();

                time0 = Time.time;
                

            }
            else if (ExtinTime > 0){
                ScaleReturn();
            }
        }
        else{
            IndicateOff();
            if(ExtinTime > 0){
                ScaleReturn();
            }
            
        }


    }

    private void ScaleReturn(){
        // here we still need deltaTime

        
        deltaTime = Time.time - time0;
        //Debug.Log(deltaTime);
        ExtinTime = ExtinTime - deltaTime;
       // Debug.Log(ExtinTime);
        // if ExtinTime <= 0
        if(ExtinTime <= 0){
            ExtinTime = 0;
        }
        ChangeScale();
        time0 = Time.time;
    }

    private void ChangeScale(){
        var scaleMul = (Adore - ExtinTime)/Adore;    
        Vector3 scale = new Vector3(originalScale.x* scaleMul,originalScale.y * scaleMul,originalScale.z * scaleMul);
        activating.transform.localScale = scale;
    }

    public void SetParent(GameObject node){
        ParentNode = node;
    }

    public void ExtinStart(){
        if(!start){
            start = true;
            time0 = Time.time;
        }
 
    }

    public void MouseOnFire(){
        mouseOnFire = true;
    }

    public void MouseExitFire(){
        mouseOnFire = false;
        start = false;
    }

    private void IndicateOn(){
        indicateFire.SetActive(true);
        realFire.SetActive(false);
        activating = indicateFire;
    }

    private void IndicateOff(){
        indicateFire.SetActive(false);
        realFire.SetActive(true);
        activating = realFire;
    }

    public void ExtinEnd(){
        start = false;
        //time0 = Time.time;
    }

    private void FireExtined(){
        //
        var node = ParentNode.GetComponent<Node>();
        node.setFireOff();
       // time0 = Time.time;
    }


}
