using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Quest", order = 1)]
public class QuestData : ScriptableObject
{
    public int id;
    public int maxMonster;
    public int monsterId;
    public string questText;
}