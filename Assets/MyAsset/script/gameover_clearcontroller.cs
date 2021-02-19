using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class gameover_clearcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
