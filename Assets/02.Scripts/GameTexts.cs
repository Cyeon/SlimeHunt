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
                text.DOText("슬라임들을 물리치고 살아남았습니다.", 2f);
                break;
            case Scene.Over:
                text.DOText("슬라임들에게 습격당해 사망하셨습니다.", 2f);
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