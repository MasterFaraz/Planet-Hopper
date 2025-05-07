using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public bool isTypeSelectedFromInspector = false;

    public GameObject plusTen, orbitSize, freeRevive;

    public GameObject particleEffect;

    public enum Type { Plus10XP, OrbitSizeIncrease, FreeRevive }

    public Type type;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 100);

        plusTen.SetActive(false);
        orbitSize.SetActive(false);
        freeRevive.SetActive(false);

        if (!isTypeSelectedFromInspector)
        {
            int random = Random.Range(0, 30);

            if (random < 15)
            {
                type = Type.Plus10XP;
                plusTen.SetActive(true);
            }
            else if((random == 17 || random == 19 || random == 21) && GameManager.instance.currentPlanet > 40
                && GameManager.instance.freeRevive == 0)
            {
                type = Type.FreeRevive;
                freeRevive.SetActive(true);
            }
            else
            {
                type = Type.OrbitSizeIncrease;
                orbitSize.SetActive(true);
            }
            
        }

        else {
            if (type == Type.Plus10XP)
            {
                plusTen.SetActive(true);
            }
            else if(type == Type.OrbitSizeIncrease)
            {
                orbitSize.SetActive(true);
            }
            else
            {
                freeRevive.SetActive(true);
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(type == Type.Plus10XP)
            {
                //Add 10 XP;
                GameManager.instance.PowerUpAdd10XPBonus();
            }
            else if (type == Type.OrbitSizeIncrease)
            {
                //Increase next orbit size;
                GameManager.instance.PowerUpIncreaseOrbitSize();
            }
            else if(type == Type.FreeRevive)
            {
                GameManager.instance.PowerUpFreeRevive();
            }

            DestroyObject();
        }
    }

    void DestroyObject()
    {
        plusTen.SetActive(false);
        orbitSize.SetActive(false);
        freeRevive.SetActive(false);

        particleEffect.SetActive(true);

        Destroy(gameObject, 3);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.firstPickupTextAlreadyShown) return;

        if(Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 5)
        {
            GameManager.instance.ShowIncomingPowerUpText();
        }
    }
}
