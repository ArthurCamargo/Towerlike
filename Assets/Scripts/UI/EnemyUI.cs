using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    Camera cameraMain;

    void Awake () {
        cameraMain = Camera.main;
    }


    void Update() {
        transform.LookAt(cameraMain.transform.position);
    }
}
