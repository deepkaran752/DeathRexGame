using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, ScoreManager> scoreboardItems = new Dictionary<Player, ScoreManager>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            ScoreBoardItemGenerator(player);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ScoreBoardItemGenerator(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreBoardItemGenerator(otherPlayer);
    }

    void ScoreBoardItemGenerator(Player player)
    {
        ScoreManager item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreManager>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    void RemoveScoreBoardItemGenerator(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    
}
