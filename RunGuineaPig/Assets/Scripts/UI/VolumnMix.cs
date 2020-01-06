using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumnMix : MonoBehaviour
{
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        var sli = gameObject.GetComponent<Slider>();
        sli.value = GlobalSettings.Volumn;
    }

    public void SetVolumn (float volumn){
        audioMixer.SetFloat("Volumn", volumn);
        GlobalSettings.Volumn = volumn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
