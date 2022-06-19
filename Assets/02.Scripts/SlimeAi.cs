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


    // 몬스터의 현재 상태
    public State state = State.IDLE;
    // 추적 사정 거리
    public float traceDist = 10.0f;
    // 공격 사정 거리
    public float attackDist = 2.0f;
    // 몬스터의 사망 여부
    public bool isDie = false;
    
    private Material faceMaterial;
    private Transform monsterTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        // 몬스터의 상태를 체크하는 코루틴 함수
        StartCoroutine(CheckMonsterState());

        // 상태에 따라 몬스터의 행동을 수행하는 코루틴 함수
        StartCoroutine(MonsterAction());
    }

    void Update()
    {
        if(agent.remainingDistance >= 2.0f)
        {
            // 에이전의 이동 회전
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

            // 몬스터 상태가 DIE 코루틴 종류
            if (state == State.DIE)
                yield break;

            if (state == State.PLAYERDIE)
                yield break;

            // 몬스터와 주인공 캐릭터 사이의 거리 측정
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