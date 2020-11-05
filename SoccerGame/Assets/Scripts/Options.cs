using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static Options instance;
    GameManager gm;
    float gameLength = 300.0f;
    public GameObject homeScreen;
    public GameObject optionsScreen;
    public Dropdown choice;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    public void SceneChange(Scene current, Scene next)
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.gameTimer = gameLength;
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickOptions()
    {
        homeScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OnClickBack()
    {
        optionsScreen.SetActive(false);
        homeScreen.SetActive(true);
        if(choice.value == 0)
        {
            gameLength = 300.0f;
        } else if (choice.value == 1)
        {
            gameLength = 420.0f;
        }
        else
        {
            gameLength = 600.0f;
        }

        Debug.Log(gameLength);

    }
}
