using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText = null;

    public List<QuestData> questList = new List<QuestData>();

    private int monsterCounts = 0;
    [SerializeField] private int index = 0;
    private QuestData curQuestData = null;

    void Start()
    {
        curQuestData = questList[index];
        SetQuestText();
    }

    public void CheckCount(int id)
    {
        if (id == curQuestData.monsterId || curQuestData.monsterId== -1)
        {
            monsterCounts++;
            SetQuestText();
        }

        if (monsterCounts == curQuestData.maxMonster)
        {
            SetNextQuest();
        }

        if (id == 5) { GameManager.GetInstance().IsGameClear = true; }

    }
    [ContextMenu("NEQUEST")]
    private void SetNextQuest()
    {
       // if (curQuestData.maxMonster != monsterCounts) { return; }
        monsterCounts = 0;
        index++;
        if(index<questList.Count) curQuestData = questList[index];
        SetQuestText();
        if (curQuestData.id == 5) { GameManager.GetInstance().IsFinalQuest = true; }
    }

    private void SetQuestText()
    {
        questText.text = curQuestData.questText + $" ({monsterCounts}/{curQuestData.maxMonster})";
    }
    
}