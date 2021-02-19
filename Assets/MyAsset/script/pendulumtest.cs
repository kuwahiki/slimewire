using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class pendulumtest : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject Visualeffect;
    private VisualEffect effect;
    private Rigidbody rigidbody;
    float gravity;
    public float timeOut;
    private float timeElapsed = 0;
    bool pendulum = true,overspeed = false;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        effect = Visualeffect.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        penduluming();
    }

    private void penduluming()
    {
        if (pendulum == true)
        {
            rigidbody.isKinematic = false;
            Vector3 movePlane = new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z);
            //張力のベクトルとその単位ベクトルを所得
            distance = target.position - this.transform.position;
            Vector3 direction = distance.normalized;

            Vector3 targetPos = distance;

            //ラジアン角を求める
            float angle = Vector3.SignedAngle(this.transform.position - target.position, Vector3.down, Vector3.right);
            float Radian = angle / 180 * Mathf.PI;

            //オブジェクトの速度を所得
            float speed = rigidbody.velocity.magnitude;

            if (this.transform.position.y <= target.position.y)
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
                    float axisangle = Vector3.SignedAngle(this.transform.position - target.position, Vector3.down, Vector3.right);
                    if (axisangle >= 0 && speed <= 3.0f)
                    {
                        addnumber = -Mathf.Sqrt(axisangle) / 50.0f;
                    }
                    if (axisangle < 0)
                    {
                        addnumber = Mathf.Sqrt(-axisangle) / 50.0f;
                    }
                    rigidbody.AddRelativeForce(Vector3.forward * 3.0f);
                }
                if (Input.GetKey(KeyCode.S) && overspeed == false)
                {
                    float addnumber = 0;
                    float axisangle = Vector3.SignedAngle(this.transform.position - target.position, Vector3.down, Vector3.right);
                    if (axisangle >= 0)
                    {
                        addnumber = Mathf.Sqrt(axisangle) / 50.0f;

                    }
                    if (axisangle < 0)
                    {
                        addnumber = -Mathf.Sqrt(-axisangle) / 50.0f;

                    }
                    rigidbody.AddRelativeForce(Vector3.back * 3.0f);
                }

                speed = rigidbody.velocity.magnitude;

                //張力を求める
                Vector3 force = direction * (Physics.gravity.magnitude * Mathf.Cos(Radian));

                //向心力を求める
                float centripetalForce = rigidbody.mass * Mathf.Pow(speed, 2) / distance.magnitude;
                if (this.transform.position.y >= target.position.y)
                {
                    force = Vector3.zero;
                    centripetalForce = 0;
                }

                rigidbody.AddForce(force, ForceMode.Acceleration);
                rigidbody.AddForce(direction * centripetalForce, ForceMode.Acceleration);

                //距離を一定に保つ
                if (distance.magnitude <= 7.0f)
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
            effect.SetVector3("target", targetPos);
            effect.SetFloat("speed", speed);
            Debug.Log(overspeed);

        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                pendulum = true;
                Visualeffect.active = true;
                rigidbody.isKinematic = true;
            }
        }
    }

    private void Endpendulum()
    {
        Visualeffect.active = false;
        if (pendulum == true)
        {
            rigidbody.AddForce(new Vector3(-distance.x, distance.y, -distance.z).normalized * 3.0f, ForceMode.VelocityChange);
        }
        pendulum = false;
    }
    private void EndEffect()
    {
        Visualeffect.active = false;
        pendulum = false;
    }
}
