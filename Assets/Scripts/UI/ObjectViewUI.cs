using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ObjectViewUI : MonoBehaviour
{
    public GameObject realObject;
    Transform prefabView;
    public Transform placeHolder;
    public Canvas canvas;
    public Image panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public RectTransform slotsHolder;

    public SocketSlot[] slots;

    public Tower currentTower;

    // Start is called before the first frame update
    void Awake()
    {   
        prefabView = null;
        slots = slotsHolder.GetComponentsInChildren<SocketSlot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(prefabView != null)
            prefabView.Rotate(new Vector3(0f, 90f, 0f) * Time.unscaledDeltaTime);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    void SetName(string text) {
        nameText.text = text;
    }

    void SetDescription(string text) { 
        descriptionText.text = text;
    }

    void UpdateItens() {
        SetItems(currentTower.equipedItems);
    }


    void SetItems(List<SocketItem> items) {
        for(int i = 0; i < items.Count; i ++)
        {
            slots[i].icon.sprite = items[i].icon;
            slots[i].icon.enabled = true;
        }
    }

    void SetPrefab(Transform prefab)
    {
        if(prefabView != null)
            GameObject.Destroy(prefabView.gameObject);

        prefabView = Instantiate(prefab, placeHolder.position, placeHolder.rotation);
        prefabView.parent = placeHolder.transform;
        prefabView.localScale += new Vector3(35f, 35f, 35f);
    }
    public void GatherTowerInformation(Transform newTransform) {
        Debug.Log("Gathering");
        SetPrefab(newTransform);
        currentTower = newTransform.GetComponent<Tower>();
        currentTower.onItemChangedCallback += UpdateItens;
        SetName(currentTower.towerTypeName);
        SetItems(currentTower.equipedItems);
    }
}
