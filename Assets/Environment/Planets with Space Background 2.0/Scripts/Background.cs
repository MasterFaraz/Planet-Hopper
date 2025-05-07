using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public float parallaxScale = 1;

    Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        Vector2 materialOffset = material.mainTextureOffset;
        materialOffset.x = transform.position.x / transform.localScale.x / parallaxScale;
        materialOffset.y = transform.position.y / transform.localScale.y / parallaxScale;
        material.mainTextureOffset = materialOffset;
    }
}
