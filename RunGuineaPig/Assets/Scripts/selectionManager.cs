using UnityEngine;
using UnityEngine.EventSystems;

public class selectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private string stopTag = "Stop";
    [SerializeField] private string turnTag = "Turn";
    [SerializeField] private string tableTag = "Table";
    //[SerializeField] private string ButtonTag = "Button";
    [SerializeField] private Material highlightMaterial;
    private Material defaultMaterial;
    private Transform _selection = null;
    
    //private var _UISelection = null;
    //Animator anim;
    // Update is called once per frame

    // here is the check for the double click
    private const float DOUBLE_CLICK_TIME = .2f;
    private float lastClickTime;
    private GameObject clicked;

    // here is global condition checker
    private int origin;
    private int building;

    void Start(){
        origin = Global.Origin;
        building = Global.Building;
    }



    void Update()
    {
        
       // allow the color back immediately?
        //if(anim != null){
              //  anim.SetBool("MouseOn", false);
               // _selection = null;
               // anim = null;
      //  }
        //else if(_selection != null)
        if(_selection != null)
        {
            var selectedObject = _selection.GetComponent<Renderer>(); 
            selectedObject.material = defaultMaterial;
            _selection = null;
        }
        

   /*     if(EventSystem.current.IsPointerOverGameObject()){
            Debug.Log("I am in!");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            if(hit.collider != null){
                Debug.Log("I Am here");
                var selection = hit.collider;
                anim = selection.GetComponent<Animator>();
                anim.SetBool("MouseOn", true);
            }
        }
        else{*/
        if(Global.Paused) return;
        if(Global.Condition == origin){
            if(!EventSystem.current.IsPointerOverGameObject()){
                //Debug.Log("I am out!");
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    //Debug.Log(1);
                    var selection = hit.transform;
                    // check whether mouse out
                    /*if(_selection == null || selection != _selection){
                        if(_selection != null){
                            var selectedObject = _selection.GetComponent<Renderer>(); 
                            selectedObject.material = defaultMaterial;
                            _selection = null;
                        }*/
                        //Debug.Log("I am new 1");



                        // check whether the object could be used by mouse
                        if (selection.CompareTag(stopTag) || selection.CompareTag(turnTag)){
                            var newSelectedObject = selection.GetComponent<Renderer>();
                            if (newSelectedObject != null)
                            {
                            // Debug.Log("I am new 2");
                                defaultMaterial = newSelectedObject.material;
                                newSelectedObject.material = highlightMaterial;
                            }
                            _selection = selection;
                            GameObject hitObj = hit.collider.gameObject;


                            // check double click
                            if(Input.GetMouseButtonDown(0)){
                                if(clicked == null){
                                    clicked = hitObj;
                                    lastClickTime = Time.time;
                                }
                                else if(clicked != hitObj){
                                    clicked = hitObj;
                                    lastClickTime = Time.time;
                                }
                                else if(clicked == hitObj){
                                    float deltaTime = Time.time - lastClickTime;
                                    // double click;
                                    if(deltaTime <= DOUBLE_CLICK_TIME){
                                        Destroy(clicked);
                                        clicked = null;
                                    }
                                    lastClickTime = Time.time;
                                }                               

                        }

                        }


                // }



                }
            }
        }
        else
        {
            //right click set the state back
            if(Input.GetMouseButtonDown(1)){
                Global.Condition = origin;
            }
        }
    }
}
