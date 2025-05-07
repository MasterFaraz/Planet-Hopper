using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoorLimits
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class CameraMoving : MonoBehaviour {

    public float speed;
    public float scrollSpeed;
    public CoorLimits cameraLimits;
    public Slider zoomSlider;
    public float tempSpeed;
    public float tempSpeedZ;

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float deltaX = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            float deltaY = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
            float deltaZ = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            transform.Translate(-deltaX, -deltaY, deltaZ);
            Clamp();
        }
    }

    void Clamp()
    {
        float zDistancing = transform.position.z - cameraLimits.zMin;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraLimits.xMin- zDistancing, cameraLimits.xMax+ zDistancing),
            Mathf.Clamp(transform.position.y, cameraLimits.yMin- zDistancing, cameraLimits.yMax+ zDistancing),
            Mathf.Clamp(transform.position.z, cameraLimits.zMin, cameraLimits.zMax));
        zoomSlider.value = transform.position.z;
    }

    public void Zoom()
    {
        Vector3 pos = transform.position;
        pos.z = zoomSlider.value;
        transform.position = pos;
    }
}
