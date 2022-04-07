using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string body;
    public string header;

    IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(0.5f);
        TooltipSystem.Show(body, header);
    }
    public void OnPointerEnter(PointerEventData eventData) {

        StartCoroutine("ShowTooltip");
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopCoroutine("ShowTooltip");  
        TooltipSystem.Hide();      
    }
}