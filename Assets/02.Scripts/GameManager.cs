using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    //���� �ؽ�Ʈ ���� ���� 
    public TMP_Text scoreText;
    //���� ���� ���� 
    private int totalScore = 0;

    //���� ������ ���� ���� 
    public GameObject monsterPrefab;

    //���� ���� ���� 
    public float createTime = 3.0f;

    //���� �⿬�� ��ġ ���� List 
    public List<Transform> points = new List<Transform>();

    //���͸� �̸� �����ؼ� ������ List
    public List<GameObject> monsterPool = new List<GameObject>();

    //������Ʈ Ǯ�� ������ ���� �ִ� ���� 
    public int maxMonsters = 10;

    //������ ���� ���θ� �����ϴ� ��� ���� 
    private bool isGameOver;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("GameManager");
                container.AddComponent<GameManager>();
            }
        }
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        /*spawnPointGroup?.GetComponents<Transform>(points); //1

        //2
        Transform[] pointArray = spawnPointGroup.GetComponentsInChildren<Transform>(true);*/

        //3 ��� ���� ���� 
        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        //���� �ð� �������� ȣ�� 
        InvokeRepeating("CreateMonsters", 2.0f, createTime);
    }
    private void CreateMonsters()
    {
        int idx = Random.Range(0, points.Count);
        //  Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
        GameObject monster = GetMonsterInPool();
        monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
        monster?.SetActive(true);
    }
    private void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; ++i)
        {
            var monster = Instantiate<GameObject>(monsterPrefab);
            monster.name = $"Monster_{i:00}";
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
    }
    public GameObject GetMonsterInPool()
    {
        foreach (var monster in monsterPool)
        {
            if (monster.activeSelf == false)
            {
                return monster;
            }
        }
        return null;
    }
    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totalScore:#,##0}</color>";
    }
}
