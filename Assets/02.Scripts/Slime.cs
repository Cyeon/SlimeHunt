using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Slime : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE
    }

    // ������ ���� ����
    public State state = State.IDLE;
    // ���� ���� �Ÿ�
    public float traceDist = 15.0f;
    // ���� ���� �Ÿ�
    public float attackDist = 1.0f;
    // ������ ��� ����
    public bool isDie = false;

    public int monsterId = 0;
    public Face faces;
    public GameObject SmileBody;
    public UnityEvent SlimeDie;

    [SerializeField]
    private int initHp = 30;
    [SerializeField]
    private int currHp = 0;

    private Material faceMaterial;
    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator anim;

    private readonly int hashTrace = Animator.StringToHash("Jump");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashHit = Animator.StringToHash("Damage");
    private readonly int hashDie = Animator.StringToHash("Die");

    void Awake()
    {
        monsterTransform = GetComponent<Transform>();
        targetTransform = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
    }
    private void OnEnable()
    {
        state = State.IDLE;

        currHp = initHp;
        isDie = false;
        GetComponent<SphereCollider>().enabled = true;
        GetComponentInChildren<SphereCollider>().enabled = true;
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(CheckMonsterState());
        // ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(MonsterAction());
    }

    void Update()
    {
        if (agent.remainingDistance >= 2.0f)
        {
            // �������� �̵� ȸ��
            Vector3 direction = agent.desiredVelocity;

            if (direction != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(direction);
                monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, rot, Time.deltaTime * 10.0f);
            }
        }
    }

    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            // ���� ���°� DIE �ڷ�ƾ ����
            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;

            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
            float distance = Vector3.Distance(monsterTransform.position, targetTransform.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    SetFace(faces.Idleface);
                    break;
                case State.TRACE:
                    agent.SetDestination(targetTransform.position);
                    agent.isStopped = false;
                    SetFace(faces.WalkFace);
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    SetFace(faces.attackFace);
                    transform.LookAt(targetTransform);
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    SetFace(faces.damageFace);
                    anim.SetTrigger(hashDie);
                    GetComponent<SphereCollider>().enabled = false;
                    GetComponentInChildren<SphereCollider>().enabled = false;
                    SlimeDie.Invoke();
                    yield return new WaitForSeconds(1.0f);
                    gameObject.SetActive(false);
                    break;
                case State.PLAYERDIE:
                    StopAllCoroutines();
                    agent.isStopped = true;
                    anim.SetTrigger("Jump");
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            Destroy(collision.gameObject);
            anim.SetTrigger(hashHit);
            // ������ hp ����
            currHp -= 10;
            if (currHp <= 0)
            {
                state = State.DIE;
            }
        }
    }
    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(monsterTransform.position, traceDist);
        }
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(monsterTransform.position, attackDist);
        }
    }
}