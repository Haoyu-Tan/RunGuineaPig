using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;
    public float speed = 25f;
    public float borderThickness = 10f;
    public float scrollSpeed = 5f;
    public float maxX;
    public float maxZ;
    public float minX;
    public float minZ;
    public float CameraY;
    
    // record original position
    private Vector3 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        //originalPosition = transform.position;
        originalPosition = new Vector3(minX,CameraY,maxZ);
        transform.position = originalPosition;
        Debug.Log(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown (KeyCode.Escape)) doMovement = !doMovement;
        if(!doMovement) return;

        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderThickness)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
            //if(transform.position.x <= minX) transform.position = new Vector3(minX,CameraY,transform.position.z); 
        }
        if(Input.GetKey("s") || Input.mousePosition.y <= borderThickness)
        {   
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            //if(transform.position.x >= maxX) transform.position = new Vector3(maxX,CameraY,transform.position.z);
        }
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderThickness)
        {
            
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            //if(transform.position.z >= maxZ) transform.position = new Vector3(transform.position.x,CameraY,maxZ);
        }
        if(Input.GetKey("a") || Input.mousePosition.x <= borderThickness)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            //if(transform.position.z <= minZ) transform.position = new Vector3(transform.position.x,CameraY,minZ);
            
        }
        if(Input.GetKey(KeyCode.Space))
        {
            transform.position = originalPosition;
        }

        // scroll
        /*float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 200 * scrollSpeed * Time.deltaTime;
        
        transform.position = pos;*/

    }

    void LateUpdate(){
        Vector3 current = transform.position;
        if(current.x >= maxX) current.x = maxX;
        else if(current.x <= minX) current.x = minX;
        if(current.z >= maxZ) current.z = maxZ;
        else if(current.z <= minZ) current.z = minZ;
        Debug.Log(current);
        transform.position = current; 

    }
    
}