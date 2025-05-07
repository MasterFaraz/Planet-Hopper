using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidClustersLeft, asteroidClustersRight;
    public Transform asteroidCluserLeftPosition, asteroidClusterRightPosition;
    public float timeIntervalBetweenClustersMin = 3, timeIntervalBetweenClustersMax = 5;
    float randomIntervalTime;
    float intervalCounter = 0;
    bool canSpawn = false;
    public bool isLeft = true, isRight = false;
    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        randomIntervalTime = Random.Range(timeIntervalBetweenClustersMin, timeIntervalBetweenClustersMax);

        int randomForLeftorRight = Random.Range(0, 10);
        if(randomForLeftorRight < 5)
        {
            isLeft = true;
            isRight = false;
        }
        else
        {
            isLeft = false;
            isRight = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn) {

            if (intervalCounter >= randomIntervalTime) {
                //Spawn a random cluster from the array
                if (isLeft)
                {
                    int randomInt = Random.Range(0, asteroidClustersLeft.Length);

                    GameObject cluster = Instantiate(asteroidClustersLeft[randomInt]);
                    cluster.transform.parent = asteroidCluserLeftPosition;
                    cluster.transform.localPosition = Vector3.zero;
                    cluster.GetComponent<AsteroidCluster>().EnableAllAsteroids();
                }
                else if (isRight) {
                    int randomInt = Random.Range(0, asteroidClustersRight.Length);

                    GameObject cluster = Instantiate(asteroidClustersRight[randomInt]);
                    cluster.transform.parent = asteroidClusterRightPosition;
                    cluster.transform.localPosition = Vector3.zero;
                    cluster.GetComponent<AsteroidCluster>().EnableAllAsteroids();
                }

                intervalCounter = 0;
                randomIntervalTime = Random.Range(timeIntervalBetweenClustersMin, timeIntervalBetweenClustersMax);
            }
            else
                intervalCounter += Time.deltaTime;
        
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Player crossed
            canSpawn = false;
            Destroy(gameObject, 10);
        }
    }
}
