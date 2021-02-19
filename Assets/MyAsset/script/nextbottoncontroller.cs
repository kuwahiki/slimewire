using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class nextbottoncontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(0.1f, 1f)
        .SetRelative(true)
        .SetEase(Ease.OutQuart)
        .SetDelay(1.0f)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void ClikOnNext()
    {
        if (this.gameObject.tag == "gameover" || this.gameObject.tag == "gameclear")
        {
            SceneManager.LoadScene("score");
        }
    }
}
