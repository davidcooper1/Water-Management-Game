using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHide : MonoBehaviour
{
    public GameObject panel;
    bool state;
    public Button button;

    void Start()
    {
        panel = GetComponent<GameObject>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchShowHide);
    }


    public void SwitchShowHide()
    {
        state = !state;
        panel.SetActive(state);
    }
}
