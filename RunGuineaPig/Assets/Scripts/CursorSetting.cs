using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetting : MonoBehaviour
{
    public static CursorSetting cInstance;
    public Texture2D originalCursor;
    public Texture2D shopCursor;
    public CursorMode curMode = CursorMode.Auto;
    public Vector2 pos = Vector2.zero;

    private int currentInt;
    private int origin;
    private int building;
    private int twoD;
    private int table;
    //[SerializeField] private GameObject toBuild;
    // Start is called before the first frame update

    void Awake()
    {
        
        if(cInstance != null)
        {
            Debug.Log("more than 1 cursor");
        }
        cInstance = this;
    }


    void Start()
    {
        //currentInt = Global.Condition;
        origin = Global.Origin;
        building = Global.Building;
        twoD = Global.TwoDem;
        table = Global.Table;
        SetOriginalCursor();
    }

    private void SetOriginalCursor(){
        Cursor.SetCursor(originalCursor, pos, curMode);
        currentInt = Global.Condition;
    }

    public void SetCur(Texture2D curTexture){
        shopCursor = curTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentInt != Global.Condition){
            if(Global.Condition == origin || Global.Condition == building || Global.Condition == table){
                SetOriginalCursor();
            }
            else{
                Cursor.SetCursor(shopCursor, pos, curMode);
                currentInt = Global.Condition;
            }
        }
    }
}
