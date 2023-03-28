using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDetection : MonoBehaviour
{
    PlayerContoller player = null;

    private void Awake()
    {
        player = GetComponentInParent<PlayerContoller>(); // since the grounded collider is applied to the child of the parent
    }

    //when the player is grounded that is touching the ground collider that we have added!
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
            return;
        player.GroundedStatus(true); 
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player.gameObject)
            return;
        player.GroundedStatus(true);
    }
    //when the player is no longer touching the ground.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject)
            return;
        player.GroundedStatus(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
            return;
        player.GroundedStatus(true);
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
            return;
        player.GroundedStatus(false);
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
            return;
        player.GroundedStatus(true);
    }
}
