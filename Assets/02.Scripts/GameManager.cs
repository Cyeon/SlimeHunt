using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxMonsters = 10;
    public int curMonsters = 0;
    public GameObject kingSlime = null;
    public GameObject healPotion = null;
    public GameObject menuPanel = null;
    public List<GameObject> monsters = new List<GameObject>();

    private float timer = 0f;
    private bool isFinalQuest = false;
    private bool isGameOver = false;
    private bool isGameClear = false;
    private static GameManager instance;

    [SerializeField]
    private float createTime = 1.5f;
    [SerializeField]
    private List<Transform> points = new List<Transform>();
    [SerializeField]
    private List<Transform> healPoints = new List<Transform>();
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
                GetComponent<LoadScenes>().LoadGameOver();
            }
        }
    }
    public bool IsGameClear
    {
        get { return isGameClear; }
        set
        {
            isGameClear = value;
            if (isGameClear)
            {
                GetComponent<LoadScenes>().LoadGameClear();
            }
        }
    }
    public bool IsFinalQuest
    {
        get { return isFinalQuest; }
        set
        {
            isFinalQuest = value;
            if (isFinalQuest)
            {
                kingSlime.SetActive(true);
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
    }

    void Start()
    {
        isFinalQuest = false;
        isGameClear = false;
        isGameOver = false;

        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("Points")?.transform;
        foreach (Transform item in spawnPointGroup)
        {
            points.Add(item);
        }
        spawnPointGroup = GameObject.Find("HealPoints")?.transform;
        foreach (Transform item in spawnPointGroup)
        {
            healPoints.Add(item);
        }
        InvokeRepeating("CreateMonster", 2.0f, createTime);
        InvokeRepeating("IncreaseMaxmonster", 61.0f, 60f);
        InvokeRepeating("CreateHealItem", 30f, 60f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (curMonsters < maxMonsters)
        {
            CreateMonsterPool();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause();
        }
    }

    private void CreateHealItem()
    {
        int idx = Random.Range(0, healPoints.Count);
        GameObject potion = Instantiate(healPotion, healPoints[idx].position, Quaternion.identity);
        potion?.SetActive(true);
    }

    private void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    private void CreateMonsterPool()
    {
        int createCount = maxMonsters - curMonsters;
        for (int i = 0; i < createCount; ++i)
        {
            int ran = Random.Range(0, monsters.Count);
            // 몬스터 생성
            //            var _monster = Instantiate<GameObject>(monster);
            var _monster = Instantiate(monsters[ran]);
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
    private void IncreaseMaxmonster()
    {
        int time = (int)timer / 60;
        int monster = time * 2 + 5;
        maxMonsters += monster;
    }

    public void SetPause()
    {
        Debug.Log("p");
        menuPanel.SetActive(!menuPanel.activeSelf);
        if (menuPanel.activeSelf == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}