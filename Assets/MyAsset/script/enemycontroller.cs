using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class enemycontroller : MonoBehaviour
{
    [SerializeField] private GameObject Lookplayer;
    [SerializeField] private VisualEffect hit;
    [SerializeField] private Transform forward;
    [SerializeField] private Transform[] targets;
    [SerializeField] private playercontroller player;
    [SerializeField] private int score,HP;
    [System.NonSerialized]public bool alive = true;
    private Animator animator;
    private float rotatespeed = 1.0f;
    private float lookangle = 2.0f,movetime = 10.0f,changeDir = 2.0f,blend;
    private int nexttarget = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        this.transform.position = targets[nexttarget].position;
        nexttarget++;
        blend = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            alive = false;
        }
        if (alive == true)
        {
            Vector3 direction = targets[nexttarget].position - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, rotatespeed * Time.deltaTime);

            Vector3 Forward = forward.position - this.transform.position;
            float angle = Vector3.Angle(Forward, direction);
            if (angle < lookangle)
            {
                transform.DOMove(targets[nexttarget].position, movetime);
                Vector3 nowdirection = targets[nexttarget].position - this.transform.position;
                if (nowdirection.magnitude > direction.magnitude * 2 / 3)
                {
                    blend += 0.1f;
                }

                if (nowdirection.magnitude < direction.magnitude / 3)
                {
                    blend -= 0.1f;
                }
            }
            else
            {
                this.blend = 0;
            }

            if (direction.magnitude < changeDir)
            {
                if (nexttarget >= targets.Length - 1)
                {
                    nexttarget = 0;
                }
                else
                {
                    nexttarget++;
                }
            }
            blend = Mathf.Min(blend, 1);
            blend = Mathf.Max(0, blend);
            animator.SetFloat("x", blend);
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,this.GetComponent<Rigidbody>().velocity.y,0);
        }
        //Debug.Log(alive);
        animator.SetBool("alive", alive);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(player.attack == true && collision.gameObject.tag == "player")
        {
            Debug.Log("hit");
            this.HP -= 1;
            if(HP <= 0)
            {
                alive = false;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0, this.GetComponent<Rigidbody>().velocity.y, 0);
                this.gameObject.tag = "diedObj";
                Invoke("DestoryThis", 5.0f);
            }
            Lookplayer.transform.LookAt(collision.transform);
            Lookplayer.transform.position = this.transform.position;
            hit.Play();
            player.addScore(this.score);
            player.attack = false;
        }
    }

    public void DestoryThis()
    {
        Destroy(this.gameObject);
    }
}
