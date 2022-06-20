using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText = null;
    
    public List<QuestData> questList = new List<QuestData>();

    private Text questContents = null;

    private int monsterCounts = 0;
    private int questCounts = 0;

    void Start()
    {
        questText = GetComponent<TextMeshProUGUI>();
    }
}
