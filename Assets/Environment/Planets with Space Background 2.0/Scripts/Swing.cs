using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour {

    public bool swing;

    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (swing)
        {
            float X = startPosition.x + (4 * Mathf.Cos(Time.time / 20));
            float Y = startPosition.y + (1.5f * Mathf.Sin(Time.time / 20));

            transform.position = new Vector3(X, Y, transform.position.z);
        }
    }
}
