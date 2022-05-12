using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextWaveUI : MonoBehaviour
{
    public int nextWaveNumber;

    public GameObject nextWaveUI;
    public GameObject button;

    public Button buttonObject;

    public List<Button> buttons;
    public SpeedController speed;
    public Image waveTypeIcon;

    public List<Image> enemyTypeImages;
    public TextMeshProUGUI[] descriptionText;
    public RectTransform descriptionPanel;
    public Wave currentWave;
    public WaveController controller;

    public InventoryUI inventoryUI;

    public void Awake () 
    { 
        buttons = new List<Button>();
    }

    public void ChooseNextWaveUI(WaveController newController, List<Wave> waveOptions)
    {
        controller = newController;
        nextWaveUI.SetActive(true);
        inventoryUI.Toogle();
        Debug.Log(waveOptions);
        
        for(int i = 0; i < waveOptions.Count; i ++)
        {

            int number = i;
            currentWave = waveOptions[i];


            if(buttons.Count < i + 1)
            {
                GameObject buttonGameObject = Instantiate(button) as GameObject;
                buttonGameObject.transform.SetParent(this.transform, false);
                buttons.Add(buttonGameObject.GetComponent<Button>());
            }
            
            buttonObject = buttons[i];
            buttonObject.onClick.AddListener(delegate {controller.ChooseWave(number);});

        
            descriptionPanel = buttonObject.GetComponentInChildren<RectTransform>();


            descriptionText = descriptionPanel.GetComponentsInChildren<TextMeshProUGUI>();
            
            waveTypeIcon = descriptionPanel.GetComponentsInChildren<Image>()[1];
            


            descriptionText[0].text = currentWave.waveType.description;
            descriptionText[0].text += "\n";

            descriptionText[1].text = "Enemy Type: \n";
            foreach(EnemyType enemyType in currentWave.enemyTypes)
            {
                descriptionText[1].text += enemyType.name + "\n";
            }

            descriptionText[2].text = currentWave.reward.ToString();

            waveTypeIcon.sprite = currentWave.waveType.icon;
            
        }
        speed.TooglePlay();
    }
    public void StartWave()
    {
        
        nextWaveUI.SetActive(false);
        speed.Resume();
    }

}
