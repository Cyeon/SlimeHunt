using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTexts : MonoBehaviour
{
    public enum Scene
    {
        none,
        Clear,
        Over
    }

    public Scene nowScene=Scene.none;
    public Text text;
    public List<GameObject> buttons = new List<GameObject>();
    private void Start()
    {
        Invoke("ButtonSet", 2.1f);
        switch (nowScene)
        {
            case Scene.none:
                break;
            case Scene.Clear:
                text.DOText("�����ӵ��� ����ġ�� ��Ƴ��ҽ��ϴ�.", 2f);
                break;
            case Scene.Over:
                text.DOText("�����ӵ鿡�� ���ݴ��� ����ϼ̽��ϴ�.", 2f);
                break;
            default:
                break;
        }
    }
    private void ButtonSet()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }
}