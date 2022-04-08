using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class WaveUI : MonoBehaviour
{
    public WaveBlock waveBlock;

    public Queue<WaveBlock> waves = new Queue<WaveBlock>();

    void Start()
    {
        for(int number = 0; number < 6; number++)
        {
            WaveBlock waveClone = Instantiate(waveBlock);
            waveClone.transform.SetParent(transform);
            waveClone.waveNumber.text = (number + 1).ToString();
            waves.Enqueue(waveClone);
        }
        
    }
    public void UpdateUI()
    {   
        int number = 0;
        ClearUI();
        foreach(WaveBlock waveBlock in waves)
        {
            WaveBlock waveClone = Instantiate(waveBlock);
            waveClone.transform.SetParent(transform);
            waveClone.waveNumber.text = (number + 1).ToString();
            number ++;
        }
    }

    public void NextWaveUI()
    {
        waves.Dequeue();
        UpdateUI();
    }

    public void ClearUI()
    {
        Debug.Log("Destroying wave block");
        foreach(WaveBlock child in GetComponentsInChildren<WaveBlock>())
        {
            Destroy(child);
        }
    }
}
