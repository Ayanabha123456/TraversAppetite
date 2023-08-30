using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* class to close/disable the Note UI */
public class CloseNote : MonoBehaviour
{
    private Button closeButton;
    
    void Start()
    {
        closeButton = transform.GetChild(1).GetComponent<Button>();
        closeButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        transform.gameObject.SetActive(false);
    }
}
