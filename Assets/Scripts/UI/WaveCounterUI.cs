using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveCounterUI : MonoBehaviour
{
    int waveNumber = 0;

    public TextMeshProUGUI waveText;


    public void ChangeWaveNumber(int number)
    {
        waveNumber = number;
        waveText.text = "Wave: " + waveNumber.ToString();
    }
}
