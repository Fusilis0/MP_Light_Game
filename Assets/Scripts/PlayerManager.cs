using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public PhotonView PV;
    public UIManager _UIManager;
    public int actorNumber;
    public PlayerController myController;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        actorNumber = PV.Owner.ActorNumber;
    }

    void Start()
    {
        if (PV.IsMine)
        {
            Debug.Log("Actor Number: " + actorNumber);
            CreateController();
        }
    }

    bool OnePlayerStanding()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        int deadPlayers = 0;
        foreach (PlayerController p in players)
        {
            if (p.gameObject.transform.localScale == Vector3.zero)
            {
                deadPlayers++;
            }
        }

        if (deadPlayers == 1)
        {
            return true;
        }
        return false;
    }

    bool allPlayersReady = false;
    void Update()
    {
        if (PV.Owner.IsMasterClient)
        {
            if (OnePlayerStanding())
            {
                RespawnAllPlayers();
            }
        }

        if (!allPlayersReady) 
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            if (players.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                allPlayersReady = true;
                StartCountdown();
            }

        }

    }

    public void StartCountdown()
    {
        PV.RPC("RPC_StartCountdown", RpcTarget.All);
    }

    [PunRPC]
    void RPC_StartCountdown()
    {
        if (PV.IsMine)
        {
            _UIManager.startCountdown = true;
        }
        
    }

    public void ResetRace(Player winner)
    {
        PV.RPC("RPC_ResetRace", RpcTarget.All, winner);
    }
    public float countdownAmount;

    [PunRPC]
    void RPC_ResetRace(Player winner)
    {
        if (PV.IsMine)
        {
            _UIManager.DisplayWinner(winner, 2);
            _UIManager.countdownTimer = countdownAmount;
            _UIManager.startCountdown = true;

            PlayerScore[] scores = FindObjectsOfType<PlayerScore>();
            foreach (PlayerScore s in scores)
            {
                if(s.PV.Owner == winner)
                {
                    s.UpdateScore(s.score + 1);
                }
            }

            myController.Respawn(myController.myRespawnPoint.position);
            myController.locked = true;

        }

    }

    void RespawnAllPlayers()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in players)
        {
            p.Respawn(p.myRespawnPoint.position);
        }
    }

    
    public void CreateController()
    {
        if (!PV.IsMine)
            return;

        GameObject controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), 
            FindObjectOfType<RespawnManager>().respawnPoints[PV.Owner.ActorNumber].position, Quaternion.identity);
        //Camera.main.GetComponent<CameraController>().target = controller.transform;
        

        myController = controller.GetComponent<PlayerController>();
        myController.playerManager = this;
        _UIManager = FindObjectOfType<UIManager>();
        _UIManager._player = controller.GetComponent<PlayerController>();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "playerScore"), Vector3.zero, Quaternion.identity);

    }

}
