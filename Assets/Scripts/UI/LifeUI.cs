using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public float maxHealth;

    public RectTransform rectTransform;
    Image lifeBar;

    private void start () { 
    }


    // Update is called once per frame
    public void UpdateUI(float health)
    {
        float xScale = health/maxHealth;
        rectTransform.localScale = new Vector3(xScale, rectTransform.localScale.y, rectTransform.localScale.z);
    }
}
