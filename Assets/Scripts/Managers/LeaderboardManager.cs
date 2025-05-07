using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardEntry;
    public Transform leaderboardTable;
    public GameObject[] tableEntry;
    public GameObject loadingText;
    public void LoadEntries()
    {
        if (leaderboardTable.childCount > 0)
        {
            foreach (Transform item in leaderboardTable)
                Destroy(item.gameObject);
        }
        

        Leaderboards.PlanetHopperLeaderboard.GetEntries(entries =>
        {
            foreach (var entry in entries)
            {
                GameObject newEntry = Instantiate(leaderboardEntry, leaderboardTable);
                LeaderboardEntry entryInfo = newEntry.GetComponent<LeaderboardEntry>();
                entryInfo.rankText.text = entry.Rank.ToString();
                entryInfo.nickNameText.text = entry.Username.ToString();
                entryInfo.scoreScoreTxt.text = entry.Score.ToString();
                entryInfo.CheckIfSelf();
            }

        });

        //tableEntry = leaderboardTable.gameObject.GetComponentsInChildren<GameObject>();
    }

    public void UpdateEntry()
    {
        string username = PlayerPrefs.GetString("Username");
        int score = PlayerPrefs.GetInt("HighScore");

        Leaderboards.PlanetHopperLeaderboard.UploadNewEntry(username, score, isSuccessful =>
        {
            if(isSuccessful) LoadEntries();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leaderboardTable.childCount > 0)
            loadingText.SetActive(false);
        else loadingText.SetActive(true);
    }
}
