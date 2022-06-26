using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 80.0f;

    private Transform playerTransform;
    //private Animation playerAnim;
    private Animator animator;

    // �ʱ� ���� ��
    private readonly float initHp = 100.0f;
    // ���� ���� ��
    public float currHp;

    // Hpbar �̹��� ����
    private Image hpBar;

    IEnumerator Start()
    {
        playerTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        rotationSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        rotationSpeed = 80.0f;

        // hp �ʱ�ȭ
        currHp = initHp;
        // hpbar �̹��� ����
        hpBar = GameObject.FindGameObjectWithTag("HPBAR")?.GetComponent<Image>();
        // �ʱ�ȭ �� HP ǥ��
        DisplayHP();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Speed", v);

        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * r * rotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PUNCH") && currHp >= 0.0f)
        {
            currHp -= 10.0f;
            Debug.Log($"Player HP = {currHp}");

            // HP ǥ��
            DisplayHP();

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }

        if (other.CompareTag("HEAL"))
        {
            currHp += 15f;
            if (currHp >= initHp) { currHp = initHp; }
            DisplayHP();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!");

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        // ���� ����
        GameManager.GetInstance().IsGameOver = true;
    }

    void DisplayHP()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}
