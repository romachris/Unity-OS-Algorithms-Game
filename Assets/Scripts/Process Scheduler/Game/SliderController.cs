using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SliderController : MonoBehaviour , IPointerDownHandler
{
    bool selected;
    public TMPro.TextMeshProUGUI size;
    public TMPro.TextMeshProUGUI priority;
    public TMPro.TextMeshProUGUI orderNum;
    public GameObject priorityGroup;
    public GameObject sizeGroup;
    public GameObject orderGroup;
    public bool allowClick;
    public GameObject lastSelected;

    private void Awake()
    {
        selected = false;
        allowClick = true;
    }
    public void display()
    {
        //Debug.Log("Click Works");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(allowClick)
        {
            display();
            selected = true;
        }
    }
    public bool getSelected()
    {
        return selected;
    }
    public void unSelectProcess()
    {
        selected = false;
    }
    public void setSize(float size)
    {
        this.size.text = size.ToString();
    } 
    public void setPriority(int priority)
    {
        this.priority.text = priority.ToString();
    }
    public void setOrderNum(float orderNum)
    {
        this.orderNum.text = orderNum.ToString();
    }
    public int getOrderNum()
    {
        return System.Convert.ToInt32(orderNum.text);
    }   
    public void hideLastSelected()
    {
        lastSelected.gameObject.SetActive(false);
    }
    public void displayLastSelected()
    {
        lastSelected.gameObject.SetActive(true) ;

    }
    public int getSize()
    {
        return System.Convert.ToInt32(size.text);
    }  
    public int getPriority()
    {
        return System.Convert.ToInt32(priority.text);
    }
    public void showOrder()
    {
        orderGroup.SetActive(true);
    } 
    public void showSize()
    {
        sizeGroup.SetActive(true);
    } 
    public void showPriority()
    {
        priorityGroup.SetActive(true);
    }

}
