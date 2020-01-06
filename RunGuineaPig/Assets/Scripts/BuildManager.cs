using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    
    private GameObject itemToBuild;

    //public GameObject ItemPrefab;

    public GameObject GetItemToBuild()
    {
        return itemToBuild;
    }

    // called before start
    void Awake()
    {
        
        if(instance != null)
        {
            Debug.Log("more than 1 build");
            return;
        }
        instance = this;
    }

    

    void Start()
    {
    //    this.itemToBuild = ItemPrefab;
    }

    public void SetToBuild(GameObject toBuild){
        itemToBuild = toBuild;
        Debug.Log("Setting!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
