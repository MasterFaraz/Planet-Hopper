using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipCamFollowController : MonoBehaviour
{
    public Transform objectToFollow;
    //public Vector3 dimensionToFollow = Vector3.up;

    float offset;

    

    public bool canFollow;

    bool isVertical, isHorizontal;

    private Vector3 velocity = Vector3.zero;
    public float smoothSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        isVertical = GameManager.instance.isVertical;
        isHorizontal = GameManager.instance.isHorizontal;

        if (isVertical)
        {
            offset = objectToFollow.position.y - transform.position.y;
        }
        else if (isHorizontal) {
            offset = objectToFollow.position.x - transform.position.x;
        }

        Debug.Log(offset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!canFollow || 
            !GameManager.instance.canCamFollowPlayer || 
            objectToFollow == null) return;

        Vector3 desiredPosition = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, objectToFollow.position.y - offset, smoothSpeed * Time.deltaTime),
            transform.position.z
        );

        // Smoothly move the camera to the desired position
        //transform.position = desiredPosition;


        if (isVertical)
        {
            transform.position = new Vector3(transform.position.x,
                    objectToFollow.position.y - offset, transform.position.z);

            //transform.position = desiredPosition;

            //transform.position = Vector3.SmoothDamp(
            //   transform.position,
            //   desiredPosition,
            //   ref velocity,
            //   0.1f
            //);

        }
        else if (isHorizontal) {
            transform.position = new Vector3(objectToFollow.position.x - offset,
                    transform.position.y, transform.position.z);
        }

        
    }

    public void ModifyCamSize(float newSize)
    {

    }

    public void RecalculateOffset()
    {
        if (isVertical)
        {
            offset = objectToFollow.position.y - transform.position.y;
        }
        else if (isHorizontal)
        {
            offset = objectToFollow.position.x - transform.position.x;
        }
    }
}
