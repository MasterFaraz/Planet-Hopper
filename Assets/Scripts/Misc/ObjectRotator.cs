using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationPlanes = Vector3.zero;
    public float rotationSpeed = 10f;

    public bool canRotate = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
            transform.Rotate(rotationPlanes * rotationSpeed * Time.deltaTime);
    }
}
