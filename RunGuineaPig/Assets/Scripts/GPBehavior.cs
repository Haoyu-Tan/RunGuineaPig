using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPBehavior : MonoBehaviour
{
    private Vector3 up = new Vector3(0,90,0);
    private Vector3 right = new Vector3(0,180,0);
    private Vector3 down = new Vector3(0,270,0);
    private Vector3 left = new Vector3(0,0,0);
    
    private Vector3 currentDirection;
    private Vector3 addOffset;



    // speed
    public float speed = 0.5f; 

    // raycast
    public float yAxisOffset = -0.01f;
    private float frontRayLength = 1.0f;
    private float sideRayLength = 1f;

    // directions
    private int dirIndex = 1;
    private int leftCheck = 0;
    private int rightCheck = 2;

    // used to check turn and move
    private bool canMove = true;
    private bool changedToRight = false;

    // used to check the ground
    private  List<GameObject> ground = new List<GameObject>();
    private int groundCount = 0;
    private bool onFire;

    //private boolean head = false;

    //private GameObject prev;
    //private GameObject next;
/////////////////////////////////////////////////////
    //self-index from 0-5, will not change
    public int index = -1;
    
    private GPManager manager;

    //turn

    private bool turn = false;

    //group variables
    private bool disableFront = false;

    public int leaderIndex = -1;

    //Vector3
    public Queue<Vector3> turnPos;

    //right: true, left:false
    public Queue<bool> turnDir;

    public Vector3 lastPos;

    public bool lastDir;


    //merge variables
    private bool priorityStop = false;
    private bool priorityTurn = false; //not using

    
    private int secretMergeDirIndex; //not using

    private Vector3 secretDestination; //not using

    private bool drop;
    public float droppingSpeed = 2f; 
    

    public GameObject ghost;
    public Vector3 destroyOffset;
///////////////////////////////////////////////////

    // tags
    [SerializeField] private string turnTag = "Turn";
    [SerializeField] private string stopTag = "Stop";
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private string gridTag = "Grid";
     [SerializeField] private string snakeTag = "Snake";

    Vector3 nextPos, destination, direction, leftPos, rightPos;
    Vector3 prevDest;


    // Start is called before the first frame update
    void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        speed = index * 0.5f;
        onFire = false;
        addOffset = new Vector3(0, 0 + yAxisOffset, 0);
        currentDirection = right;
        nextPos = Vector3.right;
        dirIndex = 1;
        destination = transform.position;
        manager = GPManager.instance;
        turnPos = new Queue<Vector3>();
        turnDir = new Queue<bool>();
    }

    // Update is called once per frame
    void Update()
    {
        if(drop){
            Drop();
            return;
        } 
        Move();
    }

    // ******************* Important ******************

    private void PigDead(){
        // you need to add something else
        player.gPNumber--;
        // guineapig leave its ground
        while (groundCount > 0){
            var node = ground[0].GetComponent<Node>();
            node.GPExit(this.gameObject);
            ground.Remove(ground[0]);
            groundCount --;
        }
        GameObject effect = (GameObject)Instantiate(ghost,transform.position + destroyOffset,transform.rotation);
        //handle with data structure
        Vector3 ghostDirection = new Vector3(0f,0f,0f);
        effect.transform.localEulerAngles = ghostDirection;
        manager.sendDyingMsg(index, leaderIndex);



        Destroy(this.gameObject);
    }

        ////////////////////////// New /////////////////////
    public void PigDeadFallingObject(){
        PigDead();
    }

    //  Guinea pig dropping
    private void Drop(){
        destination = transform.position + Vector3.down;
        transform.position = Vector3.MoveTowards(transform.position, destination, droppingSpeed * Time.deltaTime);
        if(transform.position.y < -1) PigDead();
    }



    void Move()
    {   
        /////////////////
        if(priorityStop){
            //merge happens and in merged team
            


        }
        //else if (priorityTurn){
        //        Debug.Log("destination is " + destination + " position is " + transform.position);

        //        if(Vector3.Distance(transform.position, destination) <= 0.001f){
        //            priorityTurn = false;
        //            Debug.Log("in priority turn");
                    
        //        //    Debug.Log(" prev destination is " + destination);
        //            dirIndex = secretMergeDirIndex;
        //            nextPos = Direction();
        //                // test

        //        //    CheckFront();
        //            prevDest = destination;
        //            transform.localEulerAngles = currentDirection;
        //            destination = transform.position + nextPos;

        //            Debug.Log("next destination is " + destination);

        //            manager.nextDest[index] = destination;
        //        }
        //        else{
        //            Debug.Log("in this case ");
        //            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        //            Debug.Log("still moving");
        //        }

        //}
        else{
        
            if(turnPos.Count < 1 || Vector3.Distance(turnPos.Peek(), transform.position) > 0.0001f){

                // nodes below
                if(Vector3.Distance(destination, transform.position) <= 0.0001f){
                    if(onFire) PigDead();
                    CheckTurn();
                    
                    // test
                    //addDirIndex();
                    nextPos = Direction();

                    

                    // test
                    if(!disableFront){
                        CheckFront();
                        nextPos = Direction();
                    }
                    prevDest = destination;
                    transform.localEulerAngles = currentDirection;
                    destination = transform.position + nextPos;

                    manager.nextDest[index] = destination;
                    //Debug.Log("next destination is " + manager.nextDest[index]);

                }
                //if(! canMove){
                /*if(!disableFront){
                    CheckFront();
                }*/
                //} 
                
                //if(canMove) transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                // No nodes below
            }
            else{
                //Debug.Log(index + " turn");
                if(turnDir.Peek()){
                    //turn right
                    addDirIndex();

                }
                else{
                    //turn left
                    minusDirIndex();
                }
                lastDir = turnDir.Dequeue();
                lastPos = turnPos.Dequeue();

                //if (index == 3){
                //    Debug.Log("3 lastDir is " + lastDir + ", lastPos is " + lastPos);
                //}
                nextPos = Direction();

                    

                    // test

                // CheckFront();
                    prevDest = destination;
                    transform.localEulerAngles = currentDirection;
                    destination = transform.position + nextPos;

                    manager.nextDest[index] = destination;
                    //Debug.Log("next destination is " + manager.nextDest[index]);

            }
        }
        
    }


    // This method set whether guinea pig is head
    public void SetHead(){
        
    }

    
    //public void setPrev(GameObject previous){
    //    prev = previous;
    //}

    //public void setNext(GameObject next){
    //    prev = next;
    //}

    // This method check turn for guinea pig
    void CheckTurn(){
        // check left
        bool leftChange = false;
        bool rightChange = false;
        RaycastHit hit;
        leftPos = DirectionChecker(leftCheck);
        Ray left = new Ray (transform.position + addOffset, leftPos);
        if(Physics.Raycast(left, out hit, sideRayLength))
        {
            var hitedLeft = hit.transform;

            if(hitedLeft.CompareTag(turnTag)){
                leftChange = true;
            }
        }

        // check right
        RaycastHit hitRight;
        rightPos = DirectionChecker(rightCheck);
        Ray right = new Ray (transform.position + addOffset, rightPos);
        if(Physics.Raycast(right, out hitRight, sideRayLength))
        {
            var hitedRight = hitRight.transform;
            if(hitedRight.CompareTag(turnTag)){
                if(leftChange){
                    var randomNum = Random.Range(0,2);
                    if(randomNum >= 1){
                        leftChange = false;
                        rightChange = true;
                    }
                } 
                else rightChange = true;
                
            }
        }


        // finally we turn to where we decided;
        if(rightChange) {
            addDirIndex();
            turn = true;
        }
        else if(leftChange){ 
            minusDirIndex();
            turn  = true;
        }

        if (turn){
            //check if leader
            if (index == leaderIndex){
                //send right turn signal to manager
                manager.sendTurnSignal(index, rightChange, transform.position);

            }
            else{
                //team split
                manager.sendSplitTeam(leaderIndex, index);
                manager.sendTurnSignal(index, rightChange, transform.position);

            }
            turn = false;
        }
    }



    void CheckFront(){
        //Debug.Log("Checking Front!");
        Ray myRay = new Ray (transform.position + addOffset, nextPos);
        RaycastHit hit;
        
        //Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if(Physics.Raycast(myRay, out hit, frontRayLength))
        {
            Debug.Log("Checking Front!");
            var hited = hit.transform;

            /*if(hited.CompareTag(stopTag)){
                canMove = false;

                //check if leader
                if (index == leaderIndex){
                    //send message to manager
                    manager.sendStopSignal(index);
                }
            }*/
            if(hited.CompareTag(wallTag)){
                    Debug.Log("I hit the wall");
                    bool rightChange = false;
                    RaycastHit hits;
                    leftPos = DirectionChecker(leftCheck);
                    Ray left = new Ray (transform.position + addOffset, leftPos);
                    if(Physics.Raycast(left, out hits, sideRayLength))
                    {
                        var hitedLeft = hits.transform;

                        if(hitedLeft.CompareTag(wallTag)){
                            addDirIndex();
                            rightChange = true;
                        }
                        else{
                            minusDirIndex();
                        }
                    }
                    else{
                        minusDirIndex();
                    }
                    if (index == leaderIndex){
                    //send right turn signal to manager
                        manager.sendTurnSignal(index, rightChange, transform.position);

                    }
            }
        
            // then check tag
            
        }
        /*else{
            
            if (index == leaderIndex){
                manager.sendContinueSignal(index);
            }

            canMove = true;
        }*/
        
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


    Vector3 DirectionChecker(int num){
        if(num == 0){
            return Vector3.forward;
        }
        else if(num == 1){
            return Vector3.right;
        }
        else if(num == 2){
            return Vector3.back;
        }
        else{
            return Vector3.left;
        }
    }

    void addDirIndex(){
        if(dirIndex < 3){
            dirIndex ++;
        }
        else{
            dirIndex = 0;
        }
        // change the left and right
        leftCheck = dirIndex - 1 ;
        if(leftCheck < 0) leftCheck += 4;
        rightCheck = dirIndex + 1;
        if(rightCheck > 3) rightCheck -= 4;

    }

    void minusDirIndex(){
        if(dirIndex > 0){
            dirIndex --;
        }
        else{
            dirIndex = 3;
        }

        leftCheck = dirIndex - 1 ;
        if(leftCheck < 0) leftCheck += 4;
        rightCheck = dirIndex + 1;
        if(rightCheck > 3) rightCheck -= 4;
    }


    // collision with other objects
    private void OnCollisionEnter(Collision collisionInfo){
        //Debug.Log("I collided !!!!!!!!!!!!!!!!!!!!!!!!!!!");
        var other = collisionInfo.collider;
        if(other.transform.CompareTag(turnTag)){
            //Debug.Log("Passed Check!!!!!!!!!!!!!!!!!1");
            Destroy(other.gameObject);
            return;
        }
        if(other.transform.CompareTag(gridTag)){
            var grid = other.gameObject;
            var node = grid.GetComponent<Node>();
            // check whether node is onfire;
            if(node.CheckFire()) onFire = true;
            if(node.HasStop() && !disableFront){
                if (index == leaderIndex){
                    canMove = false;
                    manager.sendStopSignal(index);
                }      
            }
            node.GPEnter(this.gameObject);
            ground.Add(grid);
            groundCount ++;
            return;
        }
        if(other.transform.CompareTag(snakeTag)){
            PigDead();
            Destroy(other.gameObject);
        }
    }

    public void StopDestroyed(){
        if (index == leaderIndex){
            canMove = true;
            manager.sendContinueSignal(index);
        }     
        Debug.Log("!!!!!!!!!!!Stop Destroyed !!!!!!!!!!"); 
    }

    private void OnCollisionExit(Collision collisionInfo){
        var other = collisionInfo.collider;
        if(other.transform.CompareTag(gridTag)){
            var grid = other.gameObject;
            var node = grid.GetComponent<Node>();
            node.GPExit(this.gameObject);
            ground.Remove(grid);
            groundCount --;
            if(groundCount == 1){
                var checking = ground[0].GetComponent<Node>();
                if(checking.CheckBreak()){
                    ground.Remove(grid);
                    groundCount--;
                }
            }
            if(groundCount <= 0){
                drop = true;
            }
            return;
        }
    }

    public bool GetCanMove(){
        return canMove;
    }

    public void SetCanMove(bool val){
        canMove = val;
    }

    public void SetFireOff(){
        if(onFire){
            onFire = false;
        }
    }

    //call this function whenever the guinea pig join a new team as a member
    public void DisableFront(){
        disableFront = true;
    }

    //call this function whenever the guinea pig form a new team and become leader
    public void ableFront(){
        disableFront = false;
    }

    //public void setTurnInfo(Vector3 dir, Vector3 pos){
    //    turnDir = dir;
    //    turnPos = pos;
    //}


    public void setPriorityStop(bool stop){
        priorityStop = stop;
        
    }

    public bool getPriorityStop(){
        return priorityStop;
    }

    public bool getPriorityTurn(){
        return priorityTurn;
    }

    public void setPriorityTurn(bool turn){
        priorityTurn = turn;
    }


    public Vector3 getDestination(){
        return destination;
    }

    public Vector3 getPrevDest(){
        return prevDest;
    }

    public int getDirIndex(){
        return dirIndex;
    }

    public void setSecretMergeIndex(int i){
        secretMergeDirIndex = i;
    }

    public void setSecretDestination(Vector3 dest){
        secretDestination = dest;
    }

    public void TrueMove(){
        if(canMove) transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

}
