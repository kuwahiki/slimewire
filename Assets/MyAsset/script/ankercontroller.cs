using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ankercontroller : MonoBehaviour
{
    [SerializeField]private playercontroller playercontroller;
    private GameObject enemy;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private float Timeout, time;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        this.Timeout = playercontroller.timeout;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.isKinematic == false)
        {
            time += Time.deltaTime;
            if(time >= Timeout)
            {
                rigidbody.isKinematic = true;
                time = 0;
                capsuleCollider.enabled = false;
                playercontroller.setPendulum(false);
                playercontroller.setSetPendulum(false);
            }
        }
        if(playercontroller.attack == true)
        {
            this.transform.position = enemy.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if(collision.gameObject.tag == "ground")
        {
            playercontroller.setPendulum(false);
            playercontroller.setSetPendulum(false);
        }
        else
        {
            rigidbody.isKinematic = true;
            playercontroller.setPendulum(true);
        }

        if(collision.gameObject.tag == "enemy")
        {
            this.enemy = collision.gameObject;
            rigidbody.isKinematic = true;
            playercontroller.attack = true;
            playercontroller.setPendulum(true);
        }

    }
}
