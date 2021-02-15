using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menucontroller : MonoBehaviour {
    private const int PLAY = 0;
    private const int SETTING = 1;
    private const int SCORE = 2;
    private const int HELP = 3;
    private const int NONE = 4;
    [SerializeField] private GameObject helpUI;
    [SerializeField] private float speed;
    private bool Fadein = true;
    private int nextScene;

    // Start is called before the first frame update
    private void Start()
    {
        this.GetComponent<CanvasGroup>().alpha = 0.0f;
        helpUI.SetActive(false);
    }

    private void Update()
    {
        if(Fadein == true)
        {
            fadein();
        }
        else
        {
            fadeout();
            Invoke("changeScene", 1.0f);
        }
    }

    public void OnClikPlay()
    {
        //SceneManager.LoadScene("play");
        nextScene = PLAY;
        Fadein = false;
    }

    public void OnClikSetting()
    {
        //SceneManager.LoadScene("setting");
        nextScene = SETTING;
        Fadein = false;
    }

    public void OnClikScore()
    {
        //SceneManager.LoadScene("score");
        nextScene = SCORE;
        Fadein = false;
    }

    public void OnClikHelp()
    {
        //SceneManager.LoadScene("help");
        nextScene = HELP;
        Fadein = false;
        helpUI.SetActive(true);
        helpUI.GetComponent<helpcontroller>().OnClikopen();
    }

    public void OnClikExit()
    {
        helpUI.GetComponent<helpcontroller>().Exit();
        Fadein = true;
        nextScene = NONE;

    }

    public void fadein()
    {
        if (this.GetComponent<CanvasGroup>().alpha <= 1.0f)
        {
            this.GetComponent<CanvasGroup>().alpha += speed;
        }
        else
        {
            this.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }
    public void fadeout()
    {
        if (nextScene == HELP)
        {
            if (this.GetComponent<CanvasGroup>().alpha >= 0.5f)
            {
                this.GetComponent<CanvasGroup>().alpha -= speed;
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
        else
        {
            if (this.GetComponent<CanvasGroup>().alpha >= 0.0f)
            {
                this.GetComponent<CanvasGroup>().alpha -= speed;
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha = 0.0f;
            }
        }
    }

    public void changeScene()
    {
        switch (nextScene)
        {
            case PLAY:
                SceneManager.LoadScene("play");
                break;
            case SETTING:
                SceneManager.LoadScene("setting");
                break;
            case SCORE:
                SceneManager.LoadScene("score");
                break;
            default:
                break;
        }
    }
}

