using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour
{
    public Text clockText;
    float seconds;
    float minutes;
    
    public void DisplayTime(float timer)
    {
        TimeDisplayConversion(timer);

        seconds = (seconds * 10) + .5f;
        seconds = seconds / 10.0f;
        seconds = Mathf.Floor(seconds);
        if (seconds < 10)
        {
            clockText.text = minutes + ":0" + seconds;
        }
        else if(seconds == 60){
            minutes++;
            clockText.text = minutes + ":00";
        }
        else
        {
            clockText.text = minutes + ":" + seconds;
        }


    }

    void TimeDisplayConversion(float timer)
    {
        seconds = timer % 60;
        minutes = (timer - seconds) / 60;
    }
}
