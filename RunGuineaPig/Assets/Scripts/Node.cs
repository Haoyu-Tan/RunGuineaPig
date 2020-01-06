using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is responsible for the grid
public class Node : MonoBehaviour
{
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private Material unplaceableMaterial;
    [SerializeField] private Material breakMaterial;
    [SerializeField] private Material fireMaterial;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private GameObject fallRoof;
    [SerializeField] private GameObject angel;

    [SerializeField] private string guineaPigTag = "Gp";
    [SerializeField] private string StopTag = "Stop";

    private Material originalMaterial;
    private Node node;

    public int originalStatus;
    private int status;

    private int isGrid = 0;
    private int isFire = 1;
    private int isBreak = 2;
    private int isMoney = 3;
    private int isEnd = 10;
     //   public bool isEnd;
   //     public bool onFire;
     //   public bool break;
    //private GameObject fire;
    //public float fireLast;
    //private float extinguishTime;

    public Vector3 positionOffset;
    public Vector3 fireOffset;
    public Vector3 destroyOffset;
    public Vector3 roofOffset;
    public Vector3 tableOffset;
    public Vector3 angelOffset;
    
    private GameObject item;
    private GameObject trueFire;
    private GameObject moneyAngel;
    private Command command;

    private int building;
    private int origin;
    private int twoDem;
    private int table;


    private bool changed;
    private Renderer rend;

    private List<GameObject> guineaPig = new List<GameObject>();
    private GameObject Snake;
    private int count = 0;
  

    private FallingController fallingControl;
    // Start is called before the first frame update
    
    void Awake(){
        status = originalStatus;
    }
    
    
    
    void Start()
    {
        //fallingControl = FallingController.fallCon;
        
        building = Global.Building;
        origin = Global.Origin;
        twoDem = Global.TwoDem;
        table = Global.Table;
        count = guineaPig.Count;
        changed = false;
        node = this;
        Snake = null;
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        if(status == isFire) StartFire();
        if(status == isBreak) StartBreak();
        if(status == isMoney) StartMoney();
    }

    public bool HasStop(){
        if(item == null) return false;
        if(item.transform.CompareTag(StopTag)){
            return true;
        }
        return false;
    }



    public void SetParent(FallingController falcon){
        fallingControl = falcon;
    }

    private void StartBreak(){
        rend.material = breakMaterial;
    }

    private void StartMoney(){
        item = (GameObject)Instantiate(angel, transform.position + angelOffset,transform.rotation);
        var angelClass = item.GetComponent<Angel>();
        angelClass.SetParent(node);
    }

    public void MoneyCollected(){
        item = null;
        ChangeStatus(isGrid);
    }

    private void StartFire(){
        rend.material = fireMaterial;
        //Debug.Log(transform.rotation);
        Debug.Log(fireOffset);
        trueFire = (GameObject)Instantiate(fire, transform.position + fireOffset,transform.rotation);
        var fb = trueFire.GetComponent<FireBehavior>();
        fb.SetParent(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Global.Start) return;
        if(Global.Paused) return;
        if(status == isFire){
            return;
        }
        if(status == isBreak){
            return;
        }
        //if(status == isMoney) return;
 
        if(changed){
            
            
            // change render
            if(Global.Condition == origin) rend.material = originalMaterial;
            else if(Global.Condition == building){
                if(count > 0 || item != null || Snake != null) rend.material = unplaceableMaterial; 
                else rend.material = placeableMaterial;
            }
            else if(Global.Condition == table){
                if(item != null) rend.material = unplaceableMaterial; 
                else rend.material = placeableMaterial;
            }
         //   else rend.material = unplaceableMaterial;
        }
        else{
            rend.material = originalMaterial;
        }
    }

    public bool CheckRoofPlaceable(){
        if(status == isGrid){
            return true;
        }
        else{
            return false;
        }
    }

    private void ChangeStatus(int newStatus){
        // needs to check fire off and count
        status = newStatus;
        fallingControl.NodeChangeStatus(this);
        // did not consider about count
        CheckPig();

    }

    public void CheckPig(){
        if(count >  0 && status == isGrid){
            fallingControl.AddGHGP(this.gameObject);
        }
        else{
            fallingControl.RemoveGHGP(this.gameObject);
        }
    }




    // We use this function to check whether this grid is on fire
    public bool CheckFire(){
        return isFire == status;
    }

    // Check whether this will break and break if it is needed
    public bool CheckBreak(){
        if(status == isBreak) {
            BreakGrid();
            return true;
        }
        return false;
    }
    
    private void BreakGrid(){
        GameObject effect = (GameObject)Instantiate(destroyEffect,transform.position + destroyOffset,transform.rotation);
        fallingControl.NodeDestroy(this);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }


    // falling roof
    public void StartRoof(){
        GameObject roof = (GameObject)Instantiate(fallRoof,transform.position + roofOffset,transform.rotation);
        var roofFall = roof.GetComponent<fall>();
        roofFall.SetParent(this);

    }

    public void RoofFell(){
        fallingControl.NodeRoofFell(this);
        // Check for GHGP

    }
    

    public void Reset(){
        item = null;
        for(int i = 0; i < count; i++){
            guineaPig[i].GetComponent<GPBehavior>().StopDestroyed();
        }       
        
        changed = false;
        ChangeStatus(isGrid);
        //Debug.Log("cleared?");
       // Debug.Log(item);
    }
 

    void OnMouseDown()
    {
        // building mode
        if(Global.Paused) return;
        if(Global.Condition == building)
        {
            if(status == isMoney) return;
            if(status == isFire){

                return;
            }
            if(item != null || count != 0 || Snake != null)
            {
                Debug.Log("donot build here!!!!!!");// cannot build or place
                return;
            }
            
            // otherwise build


            //Debug.Log(BuildManager.instance);
            GameObject itemToBuild = BuildManager.instance.GetItemToBuild();
           // Debug.Log(itemToBuild);
            if(itemToBuild == null)
            {
                return;
                Debug.Log("Import Failed");
            }
            item = (GameObject)Instantiate(itemToBuild, transform.position + positionOffset, transform.rotation);
            //Debug.Log(node);
            //Debug.Log(item);
            command = item.GetComponent<Command>();
            //Debug.Log(item.GetComponent<Command>());
            //Debug.Log(command);
            command.setParent(node);
            return;
        }
        // Fire Extinguisher mode
        if(Global.Condition == twoDem){
            if(status != isFire) return;
            var fb = trueFire.GetComponent<FireBehavior>();
            fb.ExtinStart();
        }
        // Table Mode
        if(Global.Condition == table){
            if(status == isFire){

                return;
            }
            if(item != null)
            {
                Debug.Log("donot build here!!!!!!");// cannot build or place
                return;
            }
            
            // otherwise build


            //Debug.Log(BuildManager.instance);
            GameObject itemToBuild = BuildManager.instance.GetItemToBuild();
           // Debug.Log(itemToBuild);
            if(itemToBuild == null)
            {
                return;
                Debug.Log("Import Failed");
            }
            item = (GameObject)Instantiate(itemToBuild, transform.position + tableOffset, transform.rotation);
            Global.builtNumber++;
            Global.Condition = origin;
            //Debug.Log(node);
            //Debug.Log(item);
            //command = item.GetComponent<Command>();
            //Debug.Log(item.GetComponent<Command>());
            //Debug.Log(command);
           // command.setParent(node);
            return;
        }
    }

    void OnMouseUp(){
        if(Global.Condition == twoDem){
            if(status != isFire) return;
            var fb = trueFire.GetComponent<FireBehavior>();
            fb.ExtinEnd();
        }
    }




    void OnMouseEnter(){
        
        if(Global.Condition == building || Global.Condition == table){
            // If it is placeable
         //   if(item == null && count == 0) rend.material = placeableMaterial;
        //    else rend.material = unplaceableMaterial;
            changed = true;
        }
        if(Global.Condition == twoDem){
            if(status == isFire) {
                var fb = trueFire.GetComponent<FireBehavior>();
                fb.MouseOnFire();
                //changed = true;
            }
        }
    }

    void OnMouseExit()
    {   
        if(changed){ 
      //      if(!Global.available) {
     //           rend.material = originalMaterial;
                changed = false;
      //      }
        }
        if(Global.Condition == twoDem){
            if(status == isFire) {
                var fb = trueFire.GetComponent<FireBehavior>();
                fb.MouseExitFire();
            }
        }
        
    }


    public void SnakeEnter(GameObject snake){
        Snake = snake;
    }

    public void SnakeExit(){
        Snake = null;
    }

    
    public void GPEnter(GameObject gp){
        if(status == isEnd){
            Destroy(gp);
            player.pigWin++;
            return;
        }
        // We send message to its parent
        if(count == 0){
            if(status == isGrid){
                fallingControl.AddGHGP(this.gameObject);
            }
        }
        guineaPig.Add(gp);
        count ++;
    }

    public void GPExit(GameObject gp){
        guineaPig.Remove(gp);
        count --;
        // We send message to its parent
        if(count == 0){
            if(status == isGrid){
                fallingControl.RemoveGHGP(this.gameObject);
            }
        }
    }



    public void setFireOff(){
        // firest we need to set the guinea pigs on it to be not onFire
        ChangeStatus(isGrid);
        for(int i = 0; i < count; i++){
            var gpB = guineaPig[i].GetComponent<GPBehavior>();
            gpB.SetFireOff();
        }


        // Then we need to destroy the fire gameObject;
        Destroy(trueFire);
        trueFire = null;
    }
    /*
    private void OnCollisionEnter(Collision collisionInfo){
        var other = collisionInfo.collider;
        Debug.Log("I am");
        Debug.Log(this.gameObject);
        if(other.transform.CompareTag(guineaPigTag)){
            guineaPig.Add(other.gameObject);
            count ++;
        }
    }

    private void OnCollisionExit(Collision collisionInfo){
        var other = collisionInfo.collider;
        if(other.transform.CompareTag(guineaPigTag)){
            //int index = guineaPig.IndexOf(other.gameObject);
            
            guineaPig.Remove(other.gameObject);
            count --; 
        }
    }
*/
}
