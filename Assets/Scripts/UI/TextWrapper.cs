using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class TextWrapper : MonoBehaviour
{

    public List<TextMeshProUGUI> allText;

    public LayoutElement layoutElement;

    public int characterWrapLimit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isEditor)
        {
            bool isEnabled = false;

            foreach( TextMeshProUGUI textText in allText)
            {
                isEnabled  |= (textText.text.Length > characterWrapLimit);
                

            }
            layoutElement.enabled = isEnabled;
        }
    }
}
