using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerContoller : MonoBehaviourPunCallbacks,Damageable
{

    //for health
    [SerializeField] Image healthBarImage;
    [SerializeField] GameObject ui;
    //references
    PlayerManagement playerManager;
    //variables;
    [SerializeField] GameObject Holder;
    private float horizontalAxis, verticalAxis, verticalRotation;
    private float horizontal, vertical;
    private bool isGrounded;
    private bool Space;
    private bool leftShift;
    private bool shoot;
    Vector3 smoothMoveVelocity;
    Vector3 movingAmount;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float smoothTime;

    Rigidbody rbMain = null;
    PhotonView PV = null;

    //for weapons
    [SerializeField] Item[] items;
    int index;
    int previousIndex  =-1;

    const float MaxHealth = 150f;
    float currentHealth = MaxHealth;
    private void Awake()
    {
        rbMain = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManagement>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            EquipWeapon(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rbMain);
            Destroy(ui);
        }
    }
    void Update()
    {
        if (!PV.IsMine)
            return;

        InputHandler(); //will handle the input for the vehicle!
        Movement();
        Jump();

        EquipWeaponWithNum();

        if(transform.position.y < -10f)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rbMain.MovePosition(rbMain.position + transform.TransformDirection(movingAmount) * Time.fixedDeltaTime);
        
    }

    void InputHandler()
    {
        horizontalAxis = Input.GetAxisRaw("Mouse X");
        verticalAxis = Input.GetAxisRaw("Mouse Y");
        Space = Input.GetKey(KeyCode.Space);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        leftShift = Input.GetKey(KeyCode.LeftShift);
        shoot = Input.GetMouseButtonDown(0);
    }

    void Movement()
    {
        transform.Rotate(Vector3.up * horizontalAxis * mouseSensitivity);
        verticalRotation += verticalAxis * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        Holder.transform.localEulerAngles = Vector3.left * verticalRotation;

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        movingAmount = Vector3.SmoothDamp(movingAmount, moveDirection * (leftShift ? sprintSpeed : walkSpeed),ref smoothMoveVelocity,smoothTime);
    }

    void Jump()
    {
        if(Space && isGrounded)
        {
            rbMain.AddForce(transform.up * jumpForce);
        }
    }
    
    public void GroundedStatus(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }
   
    void EquipWeapon(int _index)
    {
        if (_index == previousIndex)
            return;

        index = _index;

        items[index].itemGameObj.SetActive(true);

        if(previousIndex != -1)
        {
            items[previousIndex].itemGameObj.SetActive(false);
        }

        previousIndex = index;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("index", index);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
    void EquipWeaponWithNum()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipWeapon(i);
                Debug.Log(i);
                break;
            }
        }

        if (shoot)
        {
            items[index].Use();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipWeapon((int)changedProps["index"]);
        }
    }

    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage),PV.Owner, damage);
        //Debug.Log("took damage" + damage);
    }

    //to let the other player know that you took damage. we'll use RPCs
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBarImage.fillAmount = currentHealth / MaxHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();   
    }
}
