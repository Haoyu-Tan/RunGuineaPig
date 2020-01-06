using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    [SerializeField] private string guineaPigTag = "Gp";
    private Node parent;
    private bool isUsing;
    private GameObject angelCat;
    private GameObject box;

    //public float lastTime;
    public float speed;
    public Vector3 offSet;
    private Vector3 destination;

    public int money;


    // Start is called before the first frame update
    void Start()
    {
        box = transform.GetChild(1).gameObject;
        angelCat = transform.GetChild(0).gameObject;
        destination = angelCat.transform.position + offSet;
        isUsing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isUsing){
            Move();
        }
    }

    private void Move(){
        if(Vector3.Distance(destination, transform.position) <= 0.0001f){
            player.money = player.money + money;
            parent.MoneyCollected();
            Destroy(this.gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public void SetParent(Node node){
        parent = node;
    }

    private void OnTriggerEnter(Collider other){
        if(other.transform.CompareTag(guineaPigTag)){
            //var gp = other.gameObject.GetComponent<GPBehavior>();

            Destroy(box);
            isUsing = true;
        }
    
    }
}
