using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI sizeText;
    public TMPro.TextMeshProUGUI remainingSize;
    public UnityEngine.UI.Image lastSelectedImage;
    public bool selected;
    public void buttonSelected()
    {
        selected =  true;
    }

    public void setRemainingSize(float remSize)
    {
        remainingSize.text = remSize.ToString();
    }

    public void lastSelected(bool lastSelected)
    {
        lastSelectedImage.gameObject.SetActive(lastSelected);
    }

    public void setSize(int size)
    {
        sizeText.text = size.ToString() + " Mb";
    }
}
