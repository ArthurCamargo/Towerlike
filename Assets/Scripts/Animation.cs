using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Transform animationModel;
    public Vector3 animationPosition;
    public float duration;
    public float range;

    private float endOfDuration;
    private Transform animationInstance;
    private bool isOn = false;

    public void Start() {
        
    }

    void Update()
    {
        if(isOn) {
            if(endOfDuration <= Time.time) {
                Destroy(animationInstance.gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void SetAnimation(Transform animationModel, Vector3 animationPosition, float duration, float range) {
        this.animationModel = animationModel;
        this.animationPosition = animationPosition;
        this.duration = duration;
        this.range = range;
        this.endOfDuration = Time.time + duration;
        this.isOn = true;

        animationInstance = Instantiate(animationModel, animationPosition, Quaternion.identity);
        animationInstance.localScale = Vector3.one * range * 2;
    }
}
