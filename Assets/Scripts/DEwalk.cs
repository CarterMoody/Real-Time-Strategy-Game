using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEwalk : MonoBehaviour
{

    [SerializeField] private float unitSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Walk south
        //transform.position = transform.position + new Vector3(0, -0.003f, 0);
        transform.position += new Vector3(0, -1) * unitSpeed * Time.deltaTime;
    }


}
