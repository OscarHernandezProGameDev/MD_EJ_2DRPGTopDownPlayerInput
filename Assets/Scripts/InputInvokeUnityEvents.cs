using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInvokeUnityEvents : MonoBehaviour
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

    public void MoveExample(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void AttackExample(InputAction.CallbackContext context)
    {
        /*
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("Attack! started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("Attack! performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Attack! canceled");
                break;
        }
        */

        if (!context.started)
            return;

        Debug.Log("Attack!");
    }
}
