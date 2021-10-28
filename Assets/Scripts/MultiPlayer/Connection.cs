using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class Connection : MonoBehaviourPunCallbacks
{

    public GameObject Loading;
    public GameObject Menu;
    public GameObject _Menu;
    public InputField RoomName;
    public Text _roomName;
    public Transform playerListContent;
    public GameObject playerListItem;
    public GameObject StartGameButton;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Loading.SetActive(true);
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(2);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 20).ToString();
        Loading.SetActive(false);
        _Menu.SetActive(true);
        
    }
    public void createRoom(GameObject Menu)
    {
        PhotonNetwork.CreateRoom(RoomName.text);
        Menu.SetActive(false);
        Loading.SetActive(true);
        _Menu.SetActive(false);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created");
        Menu.SetActive(true);
        _roomName.text += PhotonNetwork.CurrentRoom.Name;
        Loading.SetActive(false);
        _Menu.SetActive(true);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Menu.SetActive(false);
        Loading.SetActive(true);
        _Menu.SetActive(false);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Loading.SetActive(true);
        _Menu.SetActive(false);
        
    }
    public override void OnJoinedRoom()
    {
        Menu.SetActive(true);
        _roomName.text += PhotonNetwork.CurrentRoom.Name;
        Loading.SetActive(false);
        _Menu.SetActive(true);
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().setUp(players[i]);
        }
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left");
        Loading.SetActive(false);
        _Menu.SetActive(true);
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().setUp(newPlayer);
    }
    public void StartGame()
    {
        Loading.SetActive(true);
        PhotonNetwork.LoadLevel(3);
    }
}
