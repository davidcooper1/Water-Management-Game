using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitySingleton : MonoBehaviour {

    public static AudioSource audio = null;

    void Awake()
    {
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
            audio.Play();
            DontDestroyOnLoad(audio);
        }
    }

    void Start()
    {

    }

    void Update()
    {
      
    }
}
