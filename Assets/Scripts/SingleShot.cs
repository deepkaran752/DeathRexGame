using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShot : Gun
{
    //references
    [SerializeField] Camera camBeingUsed;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        //Debug.Log(itemInfo.Name + "gun is being used!");
        Shoot();
    }
    void Shoot()
    {
        Ray ray = camBeingUsed.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = camBeingUsed.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log(hit.transform.name);
            hit.collider.gameObject.GetComponent<Damageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length == 0) { return; }
        else { GameObject bullet = Instantiate(prefabBullet, hitPosition, Quaternion.identity); Destroy(bullet, 12f); bullet.transform.SetParent(colliders[0].transform); }
        
    }
}
