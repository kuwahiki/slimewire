using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ankercontroller : MonoBehaviour
{
    [SerializeField]private playercontroller playercontroller;
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
            playercontroller.attack = true;
        }

    }
}
