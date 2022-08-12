using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{


    private AudioSource source;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();           // Gets the component responsible for playing Sounds
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
