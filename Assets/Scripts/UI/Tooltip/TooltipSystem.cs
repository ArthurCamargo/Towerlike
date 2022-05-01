using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    #region Singleton
    private static TooltipSystem instance;
    public Tooltip tooltip;
    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of the Tooltip System found!");
            return;
        }
        instance = this;
    }

    #endregion

    

    public static void Show(string body, string header="")
    {
        instance.tooltip.SetText(body, header);
        instance.tooltip.gameObject.SetActive(true);
        
    }

    public static void Hide()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
