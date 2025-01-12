using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]public PhotonView PV;
    Rigidbody rb;
    Animator anim;
    float respawnTimer = 0f;
    bool respawning = false;
    public TextMesh playerNameText;

    public int Health = 10;
    public bool alive = true;

    public bool locked = true;
    public PlayerManager playerManager;

    public Transform playerRig;
    public Transform playerGun;

    
    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        if (!PV.IsMine)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) 
            { 
                r.material.color = Color.red;
            }
            playerNameText.color = Color.red;
        }

        playerNameText.text = PV.Owner.NickName;

        myRespawnPoint = FindObjectOfType<RespawnManager>().respawnPoints[PV.Owner.ActorNumber];
    }
    

    Vector3 moveDir;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Vector3 lastDirection = new Vector3(1, 0, 1);
    public CharacterController controller;
    void Update()
    {
        if (!PV.IsMine)
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(!locked&&alive)
        {
            moveDir = transform.right*x+transform.forward*z;
        }
        else
        {
            moveDir = Vector3.zero;
        }
        

        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded())
            {
                Jump();
            }
        }

        if(transform.position.y<-15f&&alive)
        {
            TakeDamage(999, PV.Owner);
        }

        if(Health<=0)
        {
            playerRig.localScale = Vector3.zero;
            playerGun.localScale = Vector3.zero;
        }
        else
        {
            playerRig.localScale = Vector3.one;
            playerGun.localScale = Vector3.one;
        }
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "FinishLine")
        {
            PlayerManager[] managers = FindObjectsOfType<PlayerManager>();
            foreach (PlayerManager m in managers)
            {
                m.ResetRace(PV.Owner);
            }

        }
    }

    void UpdateAnimations()
    {
        if ((Mathf.Abs(rb.velocity.x) > 0)||(Mathf.Abs(rb.velocity.z) > 0))
        {
            anim.SetBool("run",true);
        }
        else
        {
            anim.SetBool("run", false);
        }
    }

    public Transform myRespawnPoint;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        if (respawning)
        {
            if (respawnTimer > 0) 
            {
                respawnTimer-=Time.fixedDeltaTime;
            }
            else
            {
                respawning = false;
                Respawn(myRespawnPoint.position);
            }
        }
        if(alive)
        {
            rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }

    public LayerMask layerMask;
    bool isGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit, 4, layerMask))
        {
            return true;
        }
        return false;
    }

    void Jump()
    {
        if (isGrounded()) 
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public GameObject cubeObject;

    [PunRPC]
    void CreateCube()
    {
        GameObject cube = Instantiate(cubeObject, transform.position+Vector3.up*3, Quaternion.identity);
    }
    
    public void TakeDamage(int damage, Player _player)
    {
        PV.RPC("RPC_TakeDamage", _player, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Health = 0;
            alive = false;

            respawning = true;
            respawnTimer = 4f;
        }
        Debug.Log(PV.Owner.NickName+" taken " + damage + " damage.");
    }

    public void Respawn(Vector3 pos)
    {
        PV.RPC("RPC_Respawn", RpcTarget.All, pos);
    }

    [PunRPC]
    void RPC_Respawn(Vector3 pos)
    {
        Health = 10;
        alive = true;
        transform.position = pos;
    }
}
