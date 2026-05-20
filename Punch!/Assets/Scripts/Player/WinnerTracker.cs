using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinnerTracker : MonoBehaviour
{
    public List<GameObject> playerList = new();
    [SerializeField]
    private LevelLoader levelLoader;
    //private int winningNum;
    //private ResultManager resultManager;
    [SerializeField]
    private ResultInfo _resultInfo;

    public static WinnerTracker instance {  get; private set; }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        int playersActive = 0;
        if (playerList.Count > 1)
        {
            Debug.Log("hit");
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<Player>().playerNumber = i;
            }
        }
        foreach (GameObject player in playerList)
        {
            if(player.activeSelf)
            {
                playersActive++;
            }
        }
        if(playersActive == 1)
        {
            _resultInfo.WinnerPlayerID = playerList[0].GetComponent<Player>().playerNumber;
            
            if(levelLoader != null)
            {
                levelLoader.LoadNextLevel();
            }

        }
    }

    public void AssignWinner()
    {
        foreach (GameObject player in playerList)
        {
            if (player.activeSelf)
            {
                //winningNum
            }
        }
    }
}
