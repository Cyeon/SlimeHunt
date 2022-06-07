using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMgr : MonoBehaviour
{
    // ���� �ؽ�Ʈ ���� ����
    public TMP_Text scoreText;

    // ���� ���� ����
    private int totalScore = 0;

    // ���� ������ ���� ����
    public GameObject monster;

    // ���� ���� ����
    public float createTime = 3.0f;

    // ���� �⿬�� ��ġ ���� List
    public List<Transform> points = new List<Transform>();

    // ���͸� �̸� �����ؼ� ������ List
    public List<GameObject> monsterPool = new List<GameObject>();

    // ������Ʈ Ǯ�� ������ ���� �ִ� ����
    public int maxMonsters = 10;
    public int curMonsters=10;
    // ������ ���� ���θ� �����ϴ� ��� ����
    private bool isGameOver;

    private float timer = 0f;

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

    private static GameMgr instance;

    public static GameMgr GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameMgr>();

            if (instance == null)
            {
                GameObject container = new GameObject("GameMgr");
                instance = container.AddComponent<GameMgr>();
            }
        }
        return instance;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // ���ھ� ���� ���
        DisplayScore(0);

        // ���� ������Ʈ Ǯ ����
        CreateMonsterPool();

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);

        //Transform[] pointArray = spawnPointGroup.GetComponentsInChildren<Transform>(true);

        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        // ���� �ð� �������� ȣ��
        InvokeRepeating("CreateMonster", 2.0f, createTime);
        InvokeRepeating("IncreaseMaxmonster", 61.0f, 60f);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (curMonsters < maxMonsters)
        {
            CreateMonsterPool();
        }
    }
    void IncreaseMaxmonster()
    {
        int time = (int)timer / 60;
        int l = (int)Mathf.Log10(time*10);
        maxMonsters += l;
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        int createCount = maxMonsters - curMonsters;
        for (int i = 0; i < createCount; ++i)
        {
            // ���� ����
            var _monster = Instantiate<GameObject>(monster);

            // ���� �̸� ����
            _monster.name = $"Monster_{i:00}";

            // ���� ��Ȱ��ȭ
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
        curMonsters = maxMonsters;
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if (_monster.activeSelf == false)
                return _monster;
        }

        return null;
    }

    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totalScore:#,##0}</color>";
    }
}
