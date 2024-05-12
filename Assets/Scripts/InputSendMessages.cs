using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSendMessages : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }
    // Equivale a la fase perform
    void OnAttack()
    {
        Debug.Log("Attack!");
    }
}
