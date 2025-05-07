using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public GameObject selectionBar;
    public TextMeshProUGUI rankText, nickNameText, scoreScoreTxt;

    public void CheckIfSelf()
    {
        if(nickNameText.text == PlayerPrefs.GetString("Username")) selectionBar.SetActive(true);
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
