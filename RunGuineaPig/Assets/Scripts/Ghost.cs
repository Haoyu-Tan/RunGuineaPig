using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Vector3 destination;
    public Vector3 offSet;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position + offSet;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move(){
        if(Vector3.Distance(destination, transform.position) <= 0.0001f){
            Destroy(this.gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

    }
}
