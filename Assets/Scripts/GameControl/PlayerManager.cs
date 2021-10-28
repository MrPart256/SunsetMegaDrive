using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private bool end=false;
    public GameObject Lose;
    public GameObject Win;
    private RoomManager manager;
    private bool playerAlive;
    private const byte blueTeam = 1;
    private const byte redTeam = 2;
    public bool _redTeam=false;
    public bool _blueTeam=false;
    private GameObject player;
    private PhotonTeamsManager teamsManager;
    public GameObject blueTeamSpawn;
    public GameObject redTeamSpawn;
    PhotonView PV;
    private void Awake()
    {
        manager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
       
        teamsManager = GameObject.Find("RoomManager").GetComponent<PhotonTeamsManager>();
        PV = GetComponent<PhotonView>();
        redTeamSpawn = GameObject.Find("redSpawnPoint");
        blueTeamSpawn = GameObject.Find("blueSpawnPoint");
    }

    void Start()
    {
        
        if (PV.IsMine)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber % 2 == 0)
            {
                PhotonNetwork.LocalPlayer.JoinTeam(blueTeam);_blueTeam = true;
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber % 2 != 0)
            {
                PhotonNetwork.LocalPlayer.JoinTeam(redTeam);_redTeam = true;
            }
        }
        CreateController();
    }
    private void Update()
    {
        if (PV.IsMine) { 
        if (player == null && _blueTeam && playerAlive)
        {
            Invoke("CreateController", 2);
            manager.blueTeamLifes--;
            playerAlive = false;
            PV.RPC("blueLifesMinus", RpcTarget.AllBuffered,manager.blueTeamLifes);
        }
        else if (player == null && _redTeam && playerAlive)
        {
            Invoke("CreateController", 2);
            manager.redTeamLifes--;
            playerAlive = false;
            PV.RPC("redLifesMinus", RpcTarget.AllBuffered,manager.redTeamLifes);
        }
        if (manager.blueTeamLifes <= 0 && _blueTeam && !end)
        {
            Instantiate(Lose);
            end = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(2);
        }
        if(manager.redTeamLifes<=0 && _blueTeam && !end)
        {
            Instantiate(Win);
            end = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(2);
        }
        if(manager.redTeamLifes<=0 && _redTeam && !end)
        {
            Instantiate(Lose);
            end = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(2);
        }
        if(manager.blueTeamLifes<=0 && _redTeam && !end)
        {
            Instantiate(Win);
            end = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(2);
        }
        }
    }

    private void CreateController()
    {
        Vector3 redTeamRange = redTeamSpawn.transform.localScale;
        Vector3 redTeamPosition = new Vector3(redTeamSpawn.transform.position.x, 0, redTeamSpawn.transform.position.z);
        Vector3 blueTeamRange = blueTeamSpawn.transform.localScale;
        Vector3 blueTeamPosition = new Vector3(blueTeamSpawn.transform.position.x, 0, blueTeamSpawn.transform.position.z);
        Vector3 redTeamRandomRange = new Vector3(Random.Range(-redTeamRange.x, redTeamRange.x), 0.1f, Random.Range(-redTeamRange.z, redTeamRange.z));
        Vector3 blueTeamRandomRange = new Vector3(Random.Range(-blueTeamRange.x, blueTeamRange.x), 0.1f, Random.Range(-blueTeamRange.z, blueTeamRange.z));
        if (_blueTeam==true) { 
          player =PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "bluePlayer"), blueTeamPosition + blueTeamRandomRange, Quaternion.identity);
            playerAlive = true;
        }
        else if(_redTeam==true) { 
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "redPlayer"), redTeamPosition + redTeamRandomRange, Quaternion.identity);
            playerAlive = true;
        }

    }
    [PunRPC]
    void redLifesMinus(int _redLifes)
    {
        manager.redTeamLifes = _redLifes;
    }
    void blueLifesMinus(int _blueLifes)
    {
        manager.blueTeamLifes = _blueLifes;
    }
}
