using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputInvokeCSharpEvents : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputActionMap playerBasic;
    private InputActionMap playerInverted;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction switchAction;

    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 direction;

    private void OnEnable()
    {
        playerBasic = playerInput.actions.FindActionMap("PlayerBasic");
        playerInverted = playerInput.actions.FindActionMap("PlayerInverted");

        ActivatePlayerBasic();
    }

    void SwitchActionMap(InputAction.CallbackContext context)
    {
        if (playerInput.currentActionMap == playerBasic)
            ActivatePlayerInverted();
        else if (playerInput.currentActionMap == playerInverted || playerInput.currentActionMap == null)
            ActivatePlayerBasic();
    }

    private void ActivatePlayerInverted()
    {
        playerInput.SwitchCurrentActionMap("PlayerInverted");

        moveAction = playerInverted.FindAction("Move");
        switchAction = playerInverted.FindAction("SwitchMap");

        attackAction.canceled += StopAttackExample;
        switchAction.performed += SwitchActionMap;

        Debug.Log("Cambio a Inverted Map");
    }

    private void ActivatePlayerBasic()
    {
        playerInput.SwitchCurrentActionMap("PlayerBasic");

        attackAction = playerBasic.FindAction("Attack");
        moveAction = playerBasic.FindAction("Move");
        switchAction = playerBasic.FindAction("SwitchMap");

        attackAction.performed += AttackExample;
        attackAction.canceled += StopAttackExample;
        switchAction.performed += SwitchActionMap;

        Debug.Log("Cambio a Basic Map");
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

    private void OnDisable()
    {
        attackAction.performed -= AttackExample;
        attackAction.canceled -= StopAttackExample;
        switchAction.performed -= SwitchActionMap;

        playerBasic.Disable();
    }
}
