using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.IO;

public class weapon : MonoBehaviourPun,IPunOwnershipCallbacks
{
    private const  byte reload_event = 199;
    private const byte shoot_event = 198;
    PhotonView PV;
    public int damage;
    public bool isPicked = false;
    private int notEnough;
    public float maxSpread;
    public float minSpread;
    public int magazine;
    public int magazineCurrent;
    public int ammoAmount;
    public Transform shootPosition;
    public GameObject bulletPrefab;
    public float repeatTime;
    public float time;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        
    }
    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
       /* if (obj.Code == reload_event)
        {
            object[] reload = (object[])obj.CustomData;
            ammoAmount = (int)reload[0]; 
            magazineCurrent = (int)reload[1];
            notEnough = (int)reload[2];
        }
        
        if (obj.Code == shoot_event)
        {
            object[] shoot = (object[])obj.CustomData;
            magazineCurrent = (int)shoot[0];
        }
       */
    }
    public void drop()
    {
        PV.TransferOwnership(0);
        PV.RPC("unParent", RpcTarget.AllBuffered);

    }
    public void pickUp(int ownerId)
    {
        PV.RequestOwnership();
        if (isPicked == false) {

            PV.RPC("Parenting", RpcTarget.AllBuffered, PV.ViewID, ownerId);
            
            /*
        var player = GameObject.FindGameObjectWithTag("Player");
        var weaponPos = GameObject.FindGameObjectWithTag("playerWeaponPos").GetComponent<Transform>();
        gameObject.transform.parent = player.transform;
        gameObject.transform.position = weaponPos.position;
            transform.localRotation = Quaternion.identity;*/

        }
    }
    public void OnOwnershipRequest(PhotonView photonView, Player requestingPlayer)
    {
        if (photonView != PV)
        {
            return;
        }
        Debug.Log("Я владелец");
        PV.TransferOwnership(requestingPlayer);
    }
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != PV)
        {
            return;
        }
    }
    public void reload()
    {
      
        if (ammoAmount > 0)
        {

            notEnough = magazine - magazineCurrent;
            if (notEnough < ammoAmount)
            {
                magazineCurrent = magazineCurrent + notEnough;
            }
            else
            {
                magazineCurrent = magazineCurrent + ammoAmount;
            }
            ammoAmount -= notEnough;
            notEnough = magazine;

        }
        if (ammoAmount < 0)
        {
            ammoAmount = 0;
        }
        PV.RPC("syncReload", RpcTarget.AllBuffered, magazineCurrent, ammoAmount, notEnough);
        //object[] reload = new object[]{ammoAmount,magazineCurrent,notEnough};
        //PhotonNetwork.RaiseEvent(reload_event, reload, RaiseEventOptions.Default,SendOptions.SendUnreliable);
        
    }

    public void shoot()
    {
       
        float randRotation = Random.Range(minSpread, maxSpread);
        if (Time.time > repeatTime && magazineCurrent > 0)
        {
            repeatTime = Time.time + time;
            var bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), new Vector3(shootPosition.position.x + randRotation, shootPosition.position.y, shootPosition.position.z), shootPosition.rotation); //Instantiate(bulletPrefab, new Vector3(shootPosition.position.x + randRotation, shootPosition.position.y, shootPosition.position.z), shootPosition.rotation);
            bullet.GetComponent<bulletFly>().damage = damage;
            magazineCurrent--;
        }
        
        //object[] shoot = new object[] { magazineCurrent};
       // PhotonNetwork.RaiseEvent(shoot_event, shoot, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        
    }
    [PunRPC]
    public void Parenting(int child,int parent)
    {
        var weapon = PhotonView.Find(child);
        var player = PhotonView.Find(parent);// GameObject.FindGameObjectWithTag("Player");
      //  var weaponPos = player.GetComponentInChildren<Transform>(); //GameObject.FindGameObjectWithTag("playerWeaponPos").GetComponent<Transform>();
        weapon.transform.parent = player.transform;
        weapon.transform.position = player.GetComponentInChildren<weaponPos>().transform.position;
        weapon.transform.rotation = player.GetComponentInChildren<weaponPos>().transform.rotation;
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<Rigidbody>().useGravity = false;
        isPicked = true;
    }
    [PunRPC]
    public void unParent()
    {
       
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.transform.parent = null;
        isPicked = false;
    }
    [PunRPC]
    private void syncReload(int _magazineCurrent,int _ammoAmount,int _notEnoguh)
    {
        magazineCurrent = _magazineCurrent;
        ammoAmount = _ammoAmount;
        notEnough = _notEnoguh;
    }
}


