using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static int Condition = 0;
    public static bool ConditionChanged = false;
//    public static GameObject Win;
//    public static GameObject Lose;
 //   public static GameStatus GS;

    public static int Origin = 0;
    public static int Building = 1;
    public static int TwoDem = 2;
    public static int Table = 4;
    public static int Stick = 5;

    public static bool Start;
    public static bool Paused;

    public static Button UIUsing;
    public static Shop ShopUsing;


    //public static bool available = true;
    public static int builtNumber = 0;

    public static int Selecting = 0;

    public static int SOrigin = 0;
    public static int SExting = 1;

    

    void Update(){
        if(Condition == Origin){
            if(UIUsing != null){
                Global.UIUsing.StopUsing();
            }
            else if(ShopUsing != null){
                Global.ShopUsing.StopUsing();
            }
        }
    }

    void Awake(){
        Time.timeScale = 1.0f;
        Condition = Origin;
        Paused = false;
        Start = false;
    }

}
