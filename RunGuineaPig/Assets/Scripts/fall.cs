using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall : MonoBehaviour
{
    public float preparingTime;
    private float prepare;
    private Rigidbody rigid;
    public Vector3 destroyOffset;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private string guineaPigTag = "Gp";
    [SerializeField] private string turnTag = "Turn";
    [SerializeField] private string stopTag = "Stop";
    [SerializeField] private string snakeTag = "Snake";
    [SerializeField] private string RoofTag = "Roof";

    private Node parent;

    // Start is called before the first frame update
    void Start()
    {
        prepare = preparingTime;
        rigid = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        prepare = prepare - Time.deltaTime;
        if(prepare <= 0){
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }
    }

    public void SetParent(Node node){
        parent = node;
    }

    public void FallDestroy(){
        // first we need to set the node to be placeable for the roof
        parent.RoofFell();



        GameObject effect = (GameObject)Instantiate(destroyEffect,transform.position + destroyOffset,transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if(other.transform.CompareTag(turnTag) || other.transform.CompareTag(stopTag) || other.transform.CompareTag(snakeTag)){
            Destroy(other.gameObject);
            return;
        }
        if(other.transform.CompareTag(guineaPigTag)){
            var gp = other.gameObject.GetComponent<GPBehavior>();
            gp.PigDeadFallingObject();
            return;
            
        }
        if(other.transform.CompareTag(RoofTag)){
            return;
        }
        FallDestroy();
        
    }
}
