using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyboxRandomizer : MonoBehaviour
{
    public Material[] skyboxes;
    public TextMeshProUGUI skyboxIdText;

    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, skyboxes.Length);

        skyboxIdText.text = random.ToString();

        RenderSettings.skybox = skyboxes[random];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
