using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : MonoBehaviour
{
    public Transform leftPosition, rightPosition;
    public GameObject blackHole;
    public bool isLeft, isRight, editorDir;
    

    // Start is called before the first frame update
    void Start()
    {
        if(!editorDir)
        {
            int random = Random.Range(0, 10);
            if (random < 5)
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

        

        
            
    }

    public void SetPosition()
    { 
        if (isLeft)
        {
            blackHole.transform.position = leftPosition.position;
        }
        else if (isRight)
        {
            blackHole.transform.position = rightPosition.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
