using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveBlock : MonoBehaviour
{
    public TextMeshProUGUI waveNumber;

    void Start() { 
    }

    public void SetText(string number)
    {
        waveNumber.text = number;
    }
}
