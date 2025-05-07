using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public float distance;
    public float angle;
    public float speed;

    Transform childObject;

    private void Awake()
    {
        childObject = transform.GetChild(0);
    }

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void Update()
    {

        transform.Rotate(Vector3.up * speed * Time.deltaTime,Space.Self);        

        if (childObject.localPosition.x != distance)
           childObject.localPosition = new Vector3 (distance, childObject.localPosition.y, childObject.localPosition.z);
        childObject.rotation = Quaternion.identity;
        
    }
}
