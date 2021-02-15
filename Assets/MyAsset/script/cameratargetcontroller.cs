using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameratargetcontroller : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Transform playerTans;
    private float pitch;
    // Start is called before the first frame update
    void Start()
    {
        playerTans = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerTans = player.transform;
        pitch -= Input.GetAxis("Mouse Y") * player.GetComponent<playercontroller>().RotateSpeed; //縦回転入力
        pitch = Mathf.Clamp(pitch, -80, 60); //縦回転角度制限する
        this.transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f); //回転の実行
    }
}
