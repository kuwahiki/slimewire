using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class playercontroller : MonoBehaviour
{
    private bool jump,run,runing, isground,jumping,unjump;
    [SerializeField] private GameObject anker;
    [SerializeField] private VisualEffect getscore,hit,heal,died;
    [SerializeField] private GameObject Visualeffect;
    [SerializeField] private float jumpPower = 100.0f,MotionChangeSpeed = 0.2f,JumpMoveSpeed = 0.1f;
    [SerializeField] private GameObject GameOver;
    [System.NonSerialized] public bool attack = false;
    private VisualEffect effect;
    private Vector3 distance;
    private float yaw, pitch, gravity;
    private int score;
    private int ignoretime,HP;
    private bool pendulum = false, overspeed = false,setpendulum = false,alive = true;
    private Animator animator;
    private Rigidbody rigidbody;

    public float RotateSpeed;
    public float timeout;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "play")
        {
            score = 0;
        }
        animator = this.GetComponent<Animator>();
        rigidbody = this.GetComponent<Rigidbody>();
        isground = animator.GetBool("isground");
        run = animator.GetBool("run");
        jump = false;
        jumping = false;
        unjump = false;
        ignoretime = 25;
        gravity = Physics.gravity.magnitude;
        effect = Visualeffect.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        var clipinfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        if (alive == true)
        {
            //プライヤーの回転
            yaw += Input.GetAxis("Mouse X") * RotateSpeed; //横回転入力
                                                           //pitch -= Input.GetAxis("Mouse Y") * RotateSpeed; //縦回転入力
            pitch = Mathf.Clamp(pitch, -80, 60); //縦回転角度制限する
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f); //回転の実行
            if(jump == false)
            {
                animator.SetFloat("x", 1.0f);
            }
            if (setpendulum == false)
            {
                Visualeffect.active = false;
                anker.transform.position = this.transform.position + Vector3.up;
                anker.transform.rotation = this.transform.rotation;
                //ジャンプ
                if (Input.GetKeyDown(KeyCode.Space) && isground == true)
                {
                    unjump = false;
                    jump = true;

                    animator.SetBool("jump", jump);
                    ignoretime = 25;
                    //Invoke("Jump", 0.1f);
                    animator.SetFloat("x", 0.0f);
                }

                if (Input.GetKey(KeyCode.W) && jump == false && isground == true)
                {
                    run = true;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    //this.transform.position += new Vector3(-JumpMoveSpeed, 0.0f, 0.0f);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    //this.transform.position += new Vector3(JumpMoveSpeed, 0.0f, 0.0f);
                }
                //空中の処理
                if (isground == false)
                {
                    if (unjump != true)
                    {
                        animator.SetFloat("x", 1.0f);
                        unjump = true;
                    }
                    float brend_x;
                    brend_x = animator.GetFloat("x");
                    if (jumping == true && brend_x < 1.0f)
                    {
                        brend_x += MotionChangeSpeed;
                        if(brend_x >= 1.0f)
                        {
                            brend_x = 1.0f;
                        }
                    }

                    else
                    {
                        //ジャンプ中の移動
                        if (Input.GetKey(KeyCode.W))
                        {
                            rigidbody.AddRelativeForce(0.0f, 0.0f, JumpMoveSpeed, ForceMode.VelocityChange);
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            rigidbody.AddRelativeForce(-JumpMoveSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            rigidbody.AddRelativeForce(0.0f, 0.0f, -JumpMoveSpeed, ForceMode.VelocityChange);
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            rigidbody.AddRelativeForce(JumpMoveSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
                        }
                    }


                    animator.SetFloat("x", brend_x);
                }
                else
                {
                    if (jump == false)
                    {
                        animator.SetFloat("x", 2.0f);
                    }
                    jumping = false;
                    unjump = false;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Visualeffect.active = true;
                    animator.SetFloat("x", 1.0f);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log(hit.collider.gameObject.transform.position);
                        Vector3 direction = hit.collider.gameObject.transform.position - this.transform.position;
                        anker.GetComponent<Rigidbody>().isKinematic = false;
                        anker.GetComponent<Rigidbody>().AddForce(direction.normalized * 100.0f, ForceMode.VelocityChange);
                        Invoke("OnAnkerCollider", 0.1f);
                        rigidbody.velocity = Vector3.zero;
                        setpendulum = true;
                    }
                    
                }

            }
            else
            {
                effect.SetVector3("target", anker.transform.position - this.transform.position);
            }

            animator.SetBool("run", run);
            animator.SetBool("jump", jump);
            //Debug.Log(clipinfo.clip.name);
            //Debug.Log(animator.GetBool("isground"));
            //Debug.Log(isground);
        }
        else
        {
            if (died != null)
            {
                died.Play();
            }
        }
        if (runing == false)
        {
            animator.SetBool("isground", isground);
        }

    }

    private void FixedUpdate()
    {
        Visualeffect.transform.position = this.transform.position;
        if (alive == true)
        {
            penduluming();
        }
    }

    //振り子運動
    private void penduluming()
    {
        if (pendulum == true)
        {
            //this.transform.LookAt(anker.transform);
            this.GetComponent<Animator>().applyRootMotion = false;
            rigidbody.isKinematic = false;
            //張力のベクトルとその単位ベクトルを所得
            distance = anker.transform.position - this.transform.position;
            Vector3 direction = distance.normalized;

            Vector3 targetPos = distance;

            //ラジアン角を求める
            float angle = Vector3.SignedAngle(this.transform.position - anker.transform.position, Vector3.down, Vector3.right);
            float Radian = angle / 180 * Mathf.PI;

            //オブジェクトの速度を所得
            float speed = rigidbody.velocity.magnitude;

            if (attack == true)
            {
                rigidbody.AddForce(direction * 5.0f, ForceMode.VelocityChange);
                rigidbody.AddForce(Vector3.up * gravity, ForceMode.Acceleration);

            }
            else
            {

                if (this.transform.position.y <= anker.transform.position.y)
                {
                    //プレイヤーがターゲットより低い時
                    if (distance.magnitude >= 10.0f)
                    {
                        this.transform.position += direction.normalized * (direction.magnitude / 100);
                    }
                    if (Input.GetMouseButton(0) && distance.magnitude >= 7.0f)
                    {
                        this.transform.position += direction.normalized * (direction.magnitude / 100);
                    }
                    if (Input.GetMouseButton(1))
                    {
                        Endpendulum();
                    }
                    if (Input.GetKey(KeyCode.W) && overspeed == false)
                    {

                        float addnumber = 0;
                        float axisangle = Vector3.SignedAngle(this.transform.position - anker.transform.position, Vector3.down, Vector3.right);
                        if (axisangle >= 0 && speed <= 3.0f)
                        {
                            addnumber = -Mathf.Sqrt(axisangle) / 50.0f;
                        }
                        if (axisangle < 0)
                        {
                            addnumber = Mathf.Sqrt(-axisangle) / 50.0f;
                        }
                        rigidbody.AddForce(Vector3.forward * 3.0f);
                    }
                    if (Input.GetKey(KeyCode.S) && overspeed == false)
                    {
                        float addnumber = 0;
                        float axisangle = Vector3.SignedAngle(this.transform.position - anker.transform.position, Vector3.down, Vector3.right);
                        if (axisangle >= 0)
                        {
                            addnumber = Mathf.Sqrt(axisangle) / 50.0f;

                        }
                        if (axisangle < 0)
                        {
                            addnumber = -Mathf.Sqrt(-axisangle) / 50.0f;

                        }
                        rigidbody.AddForce(Vector3.back * 3.0f);
                    }

                    speed = rigidbody.velocity.magnitude;

                    //張力を求める
                    Vector3 force = direction * (Physics.gravity.magnitude * Mathf.Cos(Radian));
                    //向心力を求める
                    float centripetalForce = rigidbody.mass * Mathf.Pow(speed, 2) / distance.magnitude;
                    if (this.transform.position.y >= anker.transform.position.y)
                    {
                        force = Vector3.zero;
                        centripetalForce = 0;
                    }

                    this.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
                    this.GetComponent<Rigidbody>().AddForce(direction * centripetalForce, ForceMode.Acceleration);
                    //Debug.Log("force" + (force + (direction * centripetalForce) + Physics.gravity));
                    //Debug.Log(rigidbody.velocity);
                    //距離を一定に保つ
                    if (distance.magnitude <= 10.0f)
                    {
                        this.transform.position -= direction * 0.1f;
                    }

                    //角度を制御する
                    speed = rigidbody.velocity.magnitude;
                    float MAXspeed = Mathf.Sqrt(Mathf.Abs(gravity * distance.magnitude));
                    if (speed >= MAXspeed)
                    {
                        rigidbody.velocity = rigidbody.velocity.normalized * MAXspeed;
                        Debug.Log("over");
                        overspeed = true;
                    }
                    if (speed <= 2.0f)
                    {
                        overspeed = false;
                    }

                    //強制的に振り子運動をやめる
                    if (distance.magnitude <= 10.0f && (angle <= -70.0f || angle > 70.0f))
                    {
                        EndEffect();
                        Debug.Log("overangle");
                    }
                    if (distance.magnitude <= 6.5f)
                    {
                        EndEffect();
                        Debug.Log("shotedistance");
                    }

                }
                else
                {
                    //プレイヤーがターゲットより高い時
                    rigidbody.AddForce(distance * 0.3f, ForceMode.VelocityChange);
                    Invoke("EndEffect", 0.3f);

                }
            }
            effect.SetVector3("target", targetPos);
            effect.SetFloat("speed", speed);

        }
        else
        {
        }
    }
    private void OnAnkerCollider()
    {
        anker.GetComponent<CapsuleCollider>().enabled = true;
    }

    //private void stratpendulum()
    //{
    //    pendulum = true;
    //    Visualeffect.active = true;
    //    rigidbody.isKinematic = true;
    //}

    //振り子運動終了時の処理
    private void Endpendulum()
    {
        anker.GetComponent<CapsuleCollider>().enabled = false;
        Visualeffect.active = false;
        if (pendulum == true)
        {
            rigidbody.AddForce(new Vector3(-distance.x, distance.y, -distance.z).normalized * 10.0f, ForceMode.VelocityChange);
        }
        this.GetComponent<Animator>().applyRootMotion = true;
        pendulum = false;
        setpendulum = false;
    }
    private void EndEffect()
    {
        Visualeffect.active = false;
        this.GetComponent<Animator>().applyRootMotion = true;
        pendulum = false;
        setpendulum = false;
    }
    public void setPendulum(bool pendulum)
    {
        this.pendulum = pendulum;
    }
    public void setSetPendulum(bool pendulum)
    {
        this.setpendulum = pendulum;
    }

    //Animation Event
    public void Jump()
    {
        if (animator.GetFloat("x") == 0.0f)
        {
            rigidbody.AddForce(Vector3.up * (this.jumpPower + Physics.gravity.y), ForceMode.VelocityChange);
            jumping = true;

        }

    }
    public void EndJump()
    {
        //jump = false;
        //animator.SetBool("jump", jump);
    }
    public void StartRun()
    {
        runing = true;
    }

    public void EndRun()
    {
        runing = false;
        run = false;
        animator.SetFloat("x", 1.0f);

        animator.SetBool("run", run);
    }


    //collision
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
                unjump = true;
                isground = true;
                jump = false;
                animator.SetBool("isground", isground);
                break;
            case "enemy":
                if (attack == true)
                {
                }
                else
                {
                    HP--;
                    hit.Play();
                }
                animator.applyRootMotion = false;
                Invoke("applyroot", 0.1f);
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddRelativeForce((Vector3.back*2 + Vector3.up) * 7.0f, ForceMode.VelocityChange);
                isground = false;
                run = false;
                animator.SetBool("isground", isground);
                pendulum = false;
                setpendulum = false;
                break;
            case "green":
                HP += 10;
                heal.Play();
                Destroy(collision.gameObject);
                break;
            case "white":
                score += 100;
                getscore.SetInt("count", 100);
                getscore.Play();
                Destroy(collision.gameObject);
                break;
            case "yellow":
                score += 1000;
                getscore.SetInt("count", 300);
                getscore.Play();
                Destroy(collision.gameObject);
                break;
            case "gameover":
                alive = false;
                GameOver.active = true;
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            unjump = true;
            isground = true;
            animator.SetBool("isground",isground);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isground = false;
            if (runing == false)
            {
                animator.SetBool("isground", isground);
            }
        }
    }
    private void applyroot()
    {
        animator.applyRootMotion = true;
    }
    public void addScore(int addscore)
    {
        this.score += addscore;
    }
    public bool getAlive()
    {
        return alive;
    }
}
