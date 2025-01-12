using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerScore : MonoBehaviour
{
    [HideInInspector] public PhotonView PV;
    public Text displayText;
    public int score = 0;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        DisplayScore();

        transform.parent = FindObjectOfType<UIManager>().scoreboard;
    }

    void DisplayScore()
    {
        displayText.text = PV.Owner.NickName + " " + score.ToString();
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        DisplayScore();

        if (score >= 1) 
        {
            RoomManager.Instance.ReturnToLobby();
        }
    }

}
