using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private string turnTag = "Turn";
    [SerializeField] private string stopTag = "Stop";
    [SerializeField] private string gridTag = "Grid";

    private GPManager gPParent;
    private GameObject[] gPSon;
    private List<GameObject> node = new List<GameObject>();
    private GameObject GuineaPig;
    private List<GameObject> ground = new List<GameObject>();
    private int groundCount;

    private float causionDistance;

    private Vector3 up = new Vector3(0,-90,0);
    private Vector3 right = new Vector3(0,0,0);
    private Vector3 down = new Vector3(0,90,0);
    private Vector3 left = new Vector3(0,180,0);
    private int dirIndex = 1;

    private bool drop;
    public float droppingSpeed;

    private Vector3 currentDirection, destination, nextPos;

    


    public int count;
    private int current;
    public float speed;
    

    // Start is called before the first frame update
    void Start()
    {
        groundCount = 0;
        drop = false;
        gPParent = GPManager.instance;
        gPSon = gPParent.guineaPigs;
        destination = transform.position;
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(drop){
            Drop();
            return;
        }
        // here calculate all the distance and walk in a regular way
        if(GuineaPig != null){
            destination = GuineaPig.transform.position;
            
        }
        
        
        if(GuineaPig == null){
            // check all the distance with guineapig
            if(Vector3.Distance(destination, transform.position) <= 0.0001f){
                current = current + 1;
                // here we need to check turn;
                if(current == count){
                    addDirIndex();
                    current = 0;
                }
                nextPos = Direction();
                transform.localEulerAngles = currentDirection;
                destination = transform.position + nextPos;
            }
            
            
            for(int i = 0; i < 6; i++){
                if(gPSon[i] != null){
                    var distance = Vector3.Distance(gPSon[i].transform.position, transform.position);
                    if(distance <= 2.0f){
                        GuineaPig = gPSon[i].gameObject;
                        destination = GuineaPig.transform.position;
                        Move();
                        return;
                    }
                }
            }


        }
        // Here we start to move our snake
        Move();

    }

    private void Move(){
        if(GuineaPig != null){
            var degree = Mathf.Atan((transform.position.y-destination.y)/( transform.position.x - destination.x));
            if(transform.position.y >= destination.y) degree = degree + 270;
            else degree = degree - 90;
            Vector3 facing = new Vector3(0.0f, degree, 0.0f);
            transform.localEulerAngles = facing;

        }
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    // right turn
    void addDirIndex(){
        if(dirIndex < 3){
            dirIndex ++;
        }
        else{
            dirIndex = 0;
        }

    }

    private void Drop(){
        destination = transform.position + Vector3.down;
        transform.position = Vector3.MoveTowards(transform.position, destination, droppingSpeed * Time.deltaTime);
        if(transform.position.y < -1) Destroy(gameObject);
    }

    Vector3 Direction(){
        if(dirIndex == 0){
            currentDirection = up;
            return Vector3.forward;
            
        }
        else if(dirIndex == 1){
            currentDirection = right;
            return Vector3.right;
            
        }
        else if(dirIndex == 2){
            currentDirection = down;
            return Vector3.back;
            
        }
        else{
            currentDirection = left;
            return Vector3.left;
           
        }
    }

    private void OnCollisionEnter(Collision collisionInfo){
        //Debug.Log("I collided !!!!!!!!!!!!!!!!!!!!!!!!!!!");
        var other = collisionInfo.collider;
        if(other.transform.CompareTag(turnTag) || other.transform.CompareTag(stopTag)){
            //Debug.Log("Passed Check!!!!!!!!!!!!!!!!!1");
            Destroy(other.gameObject);
            return;
        }
        if(other.transform.CompareTag(gridTag)){
            var grid = other.gameObject;
            var node = grid.GetComponent<Node>();
            node.SnakeEnter(this.gameObject);
            ground.Add(grid);
            groundCount ++;
            return;
        }
    }

    private void OnCollisionExit(Collision collisionInfo){
        var other = collisionInfo.collider;
        if(other.transform.CompareTag(gridTag)){
            var grid = other.gameObject;
            var node = grid.GetComponent<Node>();
            node.SnakeExit();
            ground.Remove(grid);
            groundCount --;
            if(groundCount <= 0){
                drop = true;
            }
            return;
        }
    }

    void OnMouseDown(){
        if(Global.Condition == Global.Stick){
            Global.builtNumber++;
            Global.Condition = Global.Origin;
            Destroy(this.gameObject);
        }
    }
        
}
