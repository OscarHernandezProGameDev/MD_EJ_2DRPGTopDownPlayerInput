using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputInvokeCSharpEvents : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputActionMap playerBasic;
    private InputAction moveAction;
    private InputAction attackAction;

    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 direction;

    private void OnEnable()
    {
        playerBasic = playerInput.actions.FindActionMap("PlayerBasic");
        attackAction = playerBasic.FindAction("Attack");
        //attackAction = playerInput.actions["Attack"];
        moveAction = playerBasic.FindAction("Move");

        attackAction.performed += AttackExample;
        attackAction.canceled += StopAttackExample;

        playerBasic.Enable();

        // o sin mapear el action map
        //playerInput.SwitchCurrentActionMap("PlayerBasic");
    }

    private void OnDisable()
    {
        attackAction.performed -= AttackExample;
        attackAction.canceled -= StopAttackExample;

        playerBasic.Disable();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void AttackExample(InputAction.CallbackContext context)
    {
        Debug.Log("Attack!");
    }

    void StopAttackExample(InputAction.CallbackContext context)
    {
        Debug.Log("Stop Attack!");
    }
}
