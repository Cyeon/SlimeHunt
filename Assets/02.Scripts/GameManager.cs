using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxMonsters = 10;
    public int curMonsters = 10;
    public List<GameObject> monsters = new List<GameObject>();

    private float timer = 0f;
    private bool isGameOver = false;
    private bool isFinalQuest = false;
    private static GameManager instance;

    [SerializeField]
    private float createTime = 3.0f;
    [SerializeField]
    private List<Transform> points = new List<Transform>();
    [SerializeField]
    private List<GameObject> monsterPool = new List<GameObject>();
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

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();

            if (instance == null)
            {
                GameObject container = new GameObject("GameManager");
                instance = container.AddComponent<GameManager>();
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
        
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }
        InvokeRepeating("CreateMonster", 2.0f, createTime);
        InvokeRepeating("IncreaseMaxmonster", 61.0f, 60f);

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (curMonsters < maxMonsters)
        {
            CreateMonsterPool();
        }
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
            // 몬스터 생성
            //            var _monster = Instantiate<GameObject>(monster);
            var _monster = Instantiate(monsters[Random.Range(0, monsters.Count)]);
            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";

            // 몬스터 비활성화
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
    void IncreaseMaxmonster()
    {
        int time = (int)timer / 60;
        int monster = time * 2;
        maxMonsters += monster;
    }

}