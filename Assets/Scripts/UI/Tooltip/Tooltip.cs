using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI bodyField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake () { 
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if(Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int bodyLength = bodyField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit);
        }

        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = position;
    }

    public void SetText(string body, string header = "")
    {
        if(string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        }
        else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        
        bodyField.text = body;
    }
}
