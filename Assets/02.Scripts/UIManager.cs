using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;
    private UnityAction action;

    void Start()
    {
        action = () => OnStartClick();
        //무명 메서드를 활용한 이벤트 연결 
        startButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });
        //람다식 활용한 이벤트 연결 방식
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }

    private void OnButtonClick(string name)
    {
        Debug.Log($"Click Button : {name}");
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Update()
    {
        
    }
}
