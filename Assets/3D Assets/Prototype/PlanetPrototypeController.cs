using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPrototypeController : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        ShipPrototypeController shipController = other.gameObject.GetComponent<ShipPrototypeController>();
        if(shipController != null )
        {
            //shipController.RotateAroundPlanet(this.transform);
            Debug.Log("triggerfromplanettriggered");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
