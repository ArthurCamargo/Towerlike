using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    Camera cameraMain;
    Color primary;
    Color secondary;

    void Awake () {
        cameraMain = Camera.main;
    }


    void Update() {
        
        transform.LookAt(cameraMain.transform.position, Vector3.left);
    }
}
