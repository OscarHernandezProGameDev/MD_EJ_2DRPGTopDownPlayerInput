using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InputInvokeCSharpEvents : MonoBehaviour
{
    #region  PlayerInput
    private PlayerInput playerInput;

    private InputActionMap playerBasic;
    private InputActionMap playerInverted;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction switchAction;
    #endregion

    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 direction;

    private bool lockHorizontal = false;
    private bool lockVertical = false;
    public bool tryToAttack;

    private void OnEnable()
    {
        playerBasic = playerInput.actions.FindActionMap("PlayerBasic");
        playerInverted = playerInput.actions.FindActionMap("PlayerInverted");

        ActivatePlayerBasic();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("valueY", -1);
    }

    private void Update()
    {
        direction = moveAction.ReadValue<Vector2>();

        if (direction != Vector2.zero)
        {
            animator.SetFloat("valueX", direction.x);
            animator.SetFloat("valueY", direction.y);
            animator.SetBool("isMoving", true);
        }
        else
            animator.SetBool("isMoving", false);

        if (direction.x != 0f)
        {
            lockVertical = true;
            lockHorizontal = false;
        }
        else if (direction.y != 0f)
        {
            lockHorizontal = true;
            lockVertical = false;
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = direction * speed;
        if (!lockHorizontal)
            rb.velocity = new Vector2(direction.x * speed, 0);
        else if (!lockVertical)
            rb.velocity = new Vector2(0, direction.y * speed);
    }

    void AttackExample(InputAction.CallbackContext context)
    {
        tryToAttack = true;
    }

    void StopAttackExample(InputAction.CallbackContext context)
    {
        tryToAttack = false;
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

    private void OnDisable()
    {
        attackAction.performed -= AttackExample;
        attackAction.canceled -= StopAttackExample;
        switchAction.performed -= SwitchActionMap;

        playerBasic.Disable();
    }
}
