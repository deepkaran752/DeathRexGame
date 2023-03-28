using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    Player player;
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathText;

    public void Initialize(Player player)
    {
        this.player = player;
        usernameText.text = player.NickName;
    }
}
