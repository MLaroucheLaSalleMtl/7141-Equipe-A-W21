﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsmenuui : MonoBehaviour
{
    public GameObject paneloptions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paneloptions.SetActive(true);
        }
    }
}
