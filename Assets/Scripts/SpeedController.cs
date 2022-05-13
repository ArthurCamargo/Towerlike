using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour {
    // Start is called before the first frame update

    float timeState = 1f;
    public void TooglePause() {
        if(Time.timeScale == 1f || Time.timeScale == 3f) {
            Pause();
        }
        else if (timeState == 3f)
        {
            ThreeTimes();
        }
        else if(timeState == 1f){
            Resume();
        }
    }

    public void Toogle3Times() {
        if (Time.timeScale == 0f || Time.timeScale == 1f)
        {
            ThreeTimes();
        }
        else{
            Resume();
        }
    }
    public void Pause() {
        timeState = Time.timeScale;
        Time.timeScale = 0f;
        AudioListener.pause = true;

    }
    // Update is called once per frame
    public void Resume() {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void ThreeTimes() {
        Time.timeScale = 3f;
    }
}