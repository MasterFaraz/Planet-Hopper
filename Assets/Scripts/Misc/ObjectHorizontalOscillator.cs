using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHorizontalOscillator : MonoBehaviour
{
    bool isMovingRight = false, isMovingLeft = true;

    public float horizontalOscillationRangeMax = 3, horizontalOscillationRangeMin = -3;
    public float horizontalOscilationSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x <= horizontalOscillationRangeMax)
                transform.position += Vector3.right * horizontalOscilationSpeed * Time.deltaTime;
            else
            {
                isMovingLeft = true;
                isMovingRight = false;
            }

        }
        else if (isMovingLeft)
        {
            if (transform.position.x > horizontalOscillationRangeMin)
                transform.position -= Vector3.right * horizontalOscilationSpeed * Time.deltaTime;
            else
            {
                isMovingLeft = false;
                isMovingRight = true;
            }


        }
    }
}
