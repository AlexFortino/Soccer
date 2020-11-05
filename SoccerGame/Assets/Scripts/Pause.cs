using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
                Time.timeScale = 0f;
            this.gameObject.GetComponent<Canvas>().enabled = true;
        }
    }

    public void UnPause() {
        Time.timeScale = 1f;
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
