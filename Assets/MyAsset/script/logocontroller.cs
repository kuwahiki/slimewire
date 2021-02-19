using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class logocontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.DOMove(new Vector3(602.0f, 756.5f, 0.0f), 2.0f).SetDelay(1.0f).SetEase(Ease.OutBounce);
        transform.DOScale(1.2f, 0.5f).SetDelay(3.2f).SetEase(Ease.OutElastic);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.DOScale(1.4f, 0.5f).SetDelay(3.0f).SetEase(Ease.OutElastic);
    }
}
