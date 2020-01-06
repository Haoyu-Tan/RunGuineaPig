using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject win;
    [SerializeField] public GameObject lose;
    [SerializeField] public GameObject UI;

    public static int gPNumber;
    public static int money;
    public static bool moneyChanged;
    public static int pigWin;

    public Text gPNumberText;
    public Text moneyText;
    public Text countDownText;
    public GameObject lastThreeNumbersObject;
    public Text lastThreeNumbers;
    public Text lastThreeNumbers1;
    public Text countDownMark;
    public GameObject startText;

    public GameObject falling;

    public float preparingTime;
    public float maxGameTime;

    private float prepare;
    private float gameTime; 
    private float countDown;

    private int  globalCondition;

    // 

    public void LoseGuineaPig(){
        gPNumber --;
        // if(gPNumber == 0) -> lose
    }


    public void addMoney(int amount){
        money = money + amount;
    }

    public bool checkMoney(int moneyNeeded){
        if(money >= moneyNeeded) return true;
        else return false;
    }

    public void useMoney(int amount){
        money = money - amount;
    }


    void Awake()
    {
        prepare = preparingTime;
        gameTime = maxGameTime;
        countDown = prepare;
        pigWin = 0;
        gPNumber = 6;
        money = 40;
        moneyChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        //We start with the count down
        countDown -= Time.deltaTime;
        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);
        countDownText.text = string.Format("{0:00.00}", countDown);

        //Debug.Log(Time.timeScale);
        gPNumberText.text = "x  " + gPNumber.ToString();
        moneyText.text = "x  " +money.ToString();
        if(!Global.Start){
            if(countDown <= 0f){
                countDown = gameTime;
                Global.Start = true;
                if(Global.Paused){
                    Time.timeScale = 0.0f;
                }
                startText.SetActive(false);
                countDownMark.text = "Escaping Time:";
            }
            else if(countDown <= 1f){
                lastThreeNumbersObject.SetActive(false);
                startText.SetActive(true);
            }
            else if(countDown <= 4f){
                lastThreeNumbersObject.SetActive(true);
                lastThreeNumbers.text = Mathf.Floor(countDown).ToString();
                lastThreeNumbers1.text = lastThreeNumbers.text;
            }

        }
        if(countDown <= 0f){
            falling.GetComponent<FallingController>().DesasterMode();
        }
        if(pigWin >= 1){
            //Debug.Log(Global.Win);
            //Debug.Log(Time.timeScale);
            //Debug.Log("I am here");
            Global.Condition = Global.Origin;
            Paused();
            win.SetActive(true);
            
            //UI.SetActive(false);
            return;
        }
        if(gPNumber == 0 ){
            Global.Condition = Global.Origin;
            Paused();
            lose.SetActive(true);
            //UI.SetActive(false);
            return;
        }
        
    }

    public void Paused(){
            Global.Paused = true;
            if(Global.Start) Time.timeScale = 0.0f;
            //Debug.Log("Player Pausing!!!!!!!!!!!!!!!!!!");

        // freeze time;
    }

    public void Resume(){
        Global.Paused = false;
        Time.timeScale = 1.0f;
    }

    /*public void BackToMenu(){
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(){
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }*/



}
