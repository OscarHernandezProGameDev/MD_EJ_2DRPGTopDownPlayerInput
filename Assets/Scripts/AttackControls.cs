using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackControls : MonoBehaviour
{
    [SerializeField] private InputInvokeCSharpEvents playerScript;
    [SerializeField] private GameObject[] arrows;
    private Animator animator;
    private float coolDown = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerScript.tryToAttack)
        {
            if (coolDown <= 0f)
            {
                animator.SetBool("Attack", true);
                coolDown = 0.5f;
            }
        }
        if (coolDown > 0)
            coolDown -= Time.deltaTime;
    }

    public void AttackToDown()
    {
        Debug.Log("AttackToDown");
        ChooseArrow(Vector2.down, 180);
    }

    public void AttackToUp()
    {
        Debug.Log("AttackToUp");
        ChooseArrow(Vector2.up, 0);
    }

    public void AttackToLeft()
    {
        Debug.Log("AttackToLeft");
        ChooseArrow(Vector2.left, 90);
    }

    public void AttackToRight()
    {
        Debug.Log("AttackToRight");
        ChooseArrow(Vector2.right, -90);
    }

    public void StopAttack()
    {
        Debug.Log("StopAttack");
        animator.SetBool("Attack", false);
    }

    private void ChooseArrow(Vector2 direction, float rotation)
    {
        bool launchedArrow = false;

        while (!launchedArrow)
        {
            var randomArrow = Random.Range(0, arrows.Length);
            if (!arrows[randomArrow].activeInHierarchy)
            {
                arrows[randomArrow].SetActive(true);
                arrows[randomArrow].GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation);
                arrows[randomArrow].GetComponent<Rigidbody2D>().velocity = direction * 10;
                launchedArrow = true;
            }
        }
    }
}
