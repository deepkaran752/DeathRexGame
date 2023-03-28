using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    public void OnUsername()
    {
        PhotonNetwork.NickName = username.text;
    }

}
