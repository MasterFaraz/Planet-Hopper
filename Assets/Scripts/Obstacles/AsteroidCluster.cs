using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCluster : MonoBehaviour
{
    public GameObject[] asteroids;


    public void EnableAllAsteroids()
    {
        foreach (var asteroid in asteroids) asteroid.SetActive(true);
    }

    public void DestroyCluster()
    {
        Destroy(gameObject, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var asteroid in asteroids) asteroid.SetActive(true);
        DestroyCluster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
