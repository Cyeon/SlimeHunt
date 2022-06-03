using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    //점수 텍스트 연결 변수 
    public TMP_Text scoreText;
    //점수 누적 변수 
    private int totalScore = 0;

    //몬스터 프리팹 연결 변수 
    public GameObject monsterPrefab;

    //몬스터 생성 간격 
    public float createTime = 3.0f;

    //몬스터 출연할 위치 저장 List 
    public List<Transform> points = new List<Transform>();

    //몬스터를 미리 생성해서 저장할 List
    public List<GameObject> monsterPool = new List<GameObject>();

    //오브젝트 풀에 생성할 몬스터 최대 갯수 
    public int maxMonsters = 10;

    //게임의 종료 여부를 저장하는 멤버 변수 
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

        //3 모두 같은 역할 
        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        //일정 시간 간격으로 호출 
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
