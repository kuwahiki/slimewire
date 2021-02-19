using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Scrollbar>().value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
