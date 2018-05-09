using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vol : MonoBehaviour
{
    public Slider Volume;
    public AudioSource asour;

    void Start()
    {
        asour = MyUnitySingleton.audio;
    }

    void Update()
    {
        if (asour != null)
            asour.volume = Volume.value;
    }
    
        
    


}

