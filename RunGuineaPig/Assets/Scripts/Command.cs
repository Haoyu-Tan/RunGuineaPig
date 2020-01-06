using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    private Node node;
    private int origin;
    private int building;
    private int twoD;
    private int currentStatus;
    public Vector3 facing;

    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = facing;
        origin = Global.Origin;
        building = Global.Building;
        twoD = Global.TwoDem;
        currentStatus = Global.Condition;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Global.Condition != currentStatus){
            currentStatus = Global.Condition;
            if(currentStatus == origin){
                this.gameObject.layer = 0;
            }
            else{
                this.gameObject.layer = 2;
            }
            
        }*/
    }

    public void setParent(Node parent){
       node = parent;
    }

    public void OnDestroy(){
        Debug.Log(node);
        node.Reset();

    }
}
