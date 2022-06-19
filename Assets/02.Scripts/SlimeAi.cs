using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAi : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PLAYERDIE
    }

    public Face faces;
    public GameObject SmileBody;

    public int monsterId = 0;


    // ������ ���� ����
    public State state = State.IDLE;
    // ���� ���� �Ÿ�
    public float traceDist = 10.0f;
    // ���� ���� �Ÿ�
    public float attackDist = 2.0f;
    // ������ ��� ����
    public bool isDie = false;
    
    private Material faceMaterial;
    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(CheckMonsterState());

        // ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ�ƾ �Լ�
        StartCoroutine(MonsterAction());
    }

    void Update()
    {
        if(agent.remainingDistance >= 2.0f)
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
                    SetFace(faces.Idleface);

                    break;
                case State.TRACE:
                    agent.isStopped = false;
                    SetFace(faces.WalkFace);

                    break;
                case State.ATTACK:
                    SetFace(faces.attackFace);
                    break;
                case State.DIE:
                    SetFace(faces.damageFace);
                    break;
                case State.PLAYERDIE:
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}