using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helpcontroller : MonoBehaviour
{
    private Animator animator;
    private float a = 0.0f;
    [SerializeField] float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        this.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("open") == true)
        {
            fadein();
        }
        if (animator.GetBool("open") == false)
        {
            Invoke("fadeout",0.3f);

        }
    }

    public void Exit()
    {
        bool open = false;
        animator.SetBool("open", open);
    }
    public void OnClikopen()
    {
        bool open = true;
        this.GetComponent<Animator>().SetBool("open", open);
    }

    private void exit()
    {
        if (animator.GetBool("open") == false)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void fadein()
    {
        this.GetComponent<CanvasGroup>().alpha = a;
        if (a <= 1.0f)
        {
            a += speed;
        }
        else
        {
            a = 1.0f;
        }
    }
    public void fadeout()
    {
        this.GetComponent<CanvasGroup>().alpha = a;
        if (a >= 0.0f)
        {
            a -= speed;
        }
        else
        {
            a = 0.0f;
        }
    }

}
