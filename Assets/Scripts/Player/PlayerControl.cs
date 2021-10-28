using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
public class PlayerControl : MonoBehaviour
{
    public GameObject deadPic;
    public Text blueTeam;
    public Text redTeam;
    private RoomManager manager;
    private bool canBuyBlue=false;
    private bool canBuyRed = false;
    public bool bluePlayer;
    public GameObject buyMenu;
    PhotonView PV;
    public bool noWeapon = true;
    public SphereCollider pickRange;
    public float gravitationScale;
    public int health;
    public Rigidbody rb;
    public float moveSpeed;
    private Vector3 inputMovement;
    void Awake()
    {
        manager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        rb = this.GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
            Destroy(rb);
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            Destroy(GetComponentInChildren<weaponPickUp>().gameObject);
        }    
    }
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (PV.IsMine)
        {
            shoot();
            drop();
            blueTeam.text = manager.blueTeamLifes.ToString();
            redTeam.text = manager.redTeamLifes.ToString();
        }
            die();
    }
    void FixedUpdate()
    {
        if (PV.IsMine) { 
        movement();
        }
        if (!PV.IsMine)
        {
            return;
        }
    }
    private void movement()
    {
      
        inputMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.smoothDeltaTime;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 pointToLook = ray.GetPoint(distance);
            transform.LookAt(pointToLook);
        }
        //var inputMovement = new Vector3(Input.GetAxis("Horizontal"), -(gravitationScale/10)*rb.mass, Input.GetAxis("Vertical"));
        if (rb != null) { 
            if(bluePlayer) rb.velocity = inputMovement * moveSpeed;
            if (!bluePlayer) rb.velocity = -inputMovement * moveSpeed;
        }
        // rb.transform.position += inputMovement * moveSpeed * Time.deltaTime;
        // rb.AddForce(inputMovement * moveSpeed*Time.deltaTime,ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.B) && bluePlayer && canBuyBlue)
        {
            buyMenu.SetActive(true);
        }
        else if(Input.GetKey(KeyCode.B) && !bluePlayer && canBuyRed)
        {
            buyMenu.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            buyMenu.SetActive(false);
        }
    }
    private void shoot()
    {
        if (gameObject.GetComponentInChildren<weapon>() != null)
        {
            if (Input.GetMouseButton(0) && gameObject.GetComponentInChildren<weapon>().magazineCurrent > 0)
            {
                GetComponentInChildren<СameraScrpit>().shake(1, 0.1f);
                gameObject.GetComponentInChildren<weapon>().shoot();

            }
            if (Input.GetKey(KeyCode.R))
            {
                gameObject.GetComponentInChildren<weapon>().reload();
            }
        }
    }
    private void drop()
    {
        if (gameObject.GetComponentInChildren<weapon>() != null)
        {
            if (Input.GetKey(KeyCode.G))
            {
                gameObject.GetComponentInChildren<weapon>().drop();
                noWeapon = true;
            }
        }
    }
    public void getDamage(int damage)
    {
        PV.RPC("_getDamage", RpcTarget.All, damage);
    }
    private void die()
    {
        
        if (health <= 0) { 
        deadPic.SetActive(true);
            PV.RPC("_destroyComp", RpcTarget.AllBuffered);
            if (!noWeapon) {
               gameObject.GetComponentInChildren<weapon>().drop();
            }
            Invoke("destroy", 2);
        }
    }
    private void destroy()
    {
        PV.RPC("_die", RpcTarget.All);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name== "blueSpawnPoint")
        {
            canBuyBlue = true;

        }
      
        if (other.gameObject.name == "redSpawnPoint")
        {
            canBuyRed = true;

        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        canBuyBlue = false;
        canBuyRed = false;
    }
    [PunRPC]
    private void _destroyComp()
    {
        Destroy(this.gameObject.GetComponent<MeshRenderer>());
        Destroy(this.gameObject.GetComponent<Collider>());
        Destroy(rb);
    }
    [PunRPC]
    private void _die()
    {
       
        Destroy(this.gameObject);
    }
    [PunRPC]
    public void _getDamage(int damage)
    {
        health -= damage;
    }
}
