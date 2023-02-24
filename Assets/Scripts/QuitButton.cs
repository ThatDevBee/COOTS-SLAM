using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    void Start() {
        Time.timeScale = 1;
    }
    
    public void Quit() {
        Application.Quit();
    }
}
