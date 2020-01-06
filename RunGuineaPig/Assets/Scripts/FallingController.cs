using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingController : MonoBehaviour
{
   // public static FallingController fallCon;
    public float preparingTime;
    private float prepare;
    private float current;

    public int onGP;
    public int onPlaceable;
    public int total;

    private int checker1;
    private int checker2;
    private int checker3;

    private int count;
    [SerializeField] private string gridTag = "Grid";
    private List<GameObject> gridHasGuineaPig = new List<GameObject>();
    private List<GameObject> placeable = new List<GameObject>();
    //private List<GameObject> unplaceable = new List<GameObject>();
    private int gHGPCount;
    private int placeableCount;
    //private int unplaceableCount;
    // Start is called before the first frame update
    
    public void AddGHGP(GameObject grid){
        gridHasGuineaPig.Add(grid);
        gHGPCount = gridHasGuineaPig.Count;
    }

    public void RemoveGHGP(GameObject grid){
        gridHasGuineaPig.Remove(grid);
        gHGPCount = gridHasGuineaPig.Count;
    }

    public void NodeDestroy(Node node){
        //unplaceable.Remove(node.gameObject);
        gridHasGuineaPig.Remove(node.gameObject);
        placeable.Remove(node.gameObject);
        placeableCount = placeable.Count;
        //unplaceableCount = unplaceable.Count;
    }

    private void NodeAddRoof(Node node){
        placeable.Remove(node.gameObject);
        placeableCount --;

        gridHasGuineaPig.Remove(node.gameObject);
        gHGPCount = gridHasGuineaPig.Count;
        //unplaceable.Add(node.gameObject);
        //unplaceableCount ++;
        node.StartRoof();
    }

    /*private void NHGPAddRoof(Node node){
        gridHasGuineaPig.Remove(node.gamObject);
        gHGPCount --;
        NodeAddRoof(node);
    }*/


    public void NodeRoofFell(Node node){
        //unplaceable.Remove(node.gameObject);
        NodeCheckStatus(node);

    }

    public void NodeChangeStatus(Node node){
        //unplaceable.Remove(node.gameObject);
        placeable.Remove(node.gameObject);
        NodeCheckStatus(node);
    }



    private void NodeCheckStatus(Node node){
        // here we check whether the node is still placeable
        bool checkerStatus = node.CheckRoofPlaceable();
        var grid = node.gameObject;
            // Placeable
        if(checkerStatus){
            placeable.Add(grid);
            placeableCount = placeable.Count;

            // here we need to check whether it has pig
            node.CheckPig();
        }
        /*else{
            unplaceable.Add(grid);
            unplaceableCount = unplaceable.Count;
        }*/
                

    }

    void Start()
    {

        prepare = preparingTime;
        checker1 = onGP;
        checker2 = onPlaceable + onGP;
        checker3 = total;
        gHGPCount = 0;
        count = transform.childCount;
        for(int i = 0; i < count; i ++){
            var checkItem = transform.GetChild(i).gameObject;
            if(checkItem.transform.CompareTag(gridTag)){
                var checker = checkItem.GetComponent<Node>();
                checker.SetParent(this);
                NodeCheckStatus(checker);
            }
 
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(!Global.Start) return;
        current = current + Time.deltaTime;
        if(current >= prepare){
            int randomNum = Random.Range(0, checker3);

            // On guineaPig
            if(randomNum <= checker1){
                if(gHGPCount > 0){
                    randomNum = Random.Range(0,gHGPCount);
                    var grid = gridHasGuineaPig[randomNum];
                    var nodeToPlace = grid.GetComponent<Node>();
                    NodeAddRoof(nodeToPlace);
                }
            }
                        // On placeable
            else if(randomNum <= checker2){
                if(placeableCount > 0){
                    randomNum = Random.Range(0,placeableCount);
                    var grid = placeable[randomNum];
                    var nodeToPlace = grid.GetComponent<Node>();
                    NodeAddRoof(nodeToPlace);
                }
            }
            current = 0f;
        }



        // here we try to put the roof on guinea pig;

    }

    public void DesasterMode(){
            checker1 = 2;
            checker2 = 98;
            prepare = 0;
            checker3 = checker1 + checker2;
    }
}
