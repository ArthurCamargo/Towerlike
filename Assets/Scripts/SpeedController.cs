using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour {
    // Start is called before the first frame update


    public void TooglePlay() {
        if(Time.timeScale == 0f) {
            Resume();
        }
        else {
            Pause();
        }
    }
    public void Pause() {
        Time.timeScale = 0f;
    }
    // Update is called once per frame
    public void Resume() {
        Time.timeScale = 1f;
    }

    public void ThreeTimes() {
        Time.timeScale = 3f;
    }
}