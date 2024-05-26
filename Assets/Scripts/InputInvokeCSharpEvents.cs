using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private InputActionMap mapUI;
    private InputActionMap mapSwitcher;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction switchAction;
    private InputAction menuActivateDeactivate;
    #endregion

    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject canvasObj;
    private Vector2 direction;

    private bool lockHorizontal = false;
    private bool lockVertical = false;
    public bool tryToAttack;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("valueY", -1);
    }

    private void OnEnable()
    {
        playerBasic = playerInput.actions.FindActionMap("PlayerBasic");
        playerInverted = playerInput.actions.FindActionMap("PlayerInverted");

        attackAction = playerBasic.FindAction("Attack");
        attackAction.performed += AttackExample;
        attackAction.canceled += StopAttackExample;

        mapUI = playerInput.actions.FindActionMap("UI");
        mapSwitcher = playerInput.actions.FindActionMap("MapSwitcher");

        if (canvasObj != null)
        {
            mapUI.Disable();
            menuActivateDeactivate = mapSwitcher.FindAction("Menu");

            menuActivateDeactivate.performed += MenuControl;

            mapSwitcher.Enable();
        }
        ActivatePlayerBasic();
    }

    void OnGUI()
    {
        GUILayout.TextArea($"Current map: {playerInput.currentActionMap.name}");
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

        DesSuscribeActions();

        moveAction = playerInverted.FindAction("Move");
        switchAction = playerInverted.FindAction("SwitchMap");

        switchAction.performed += SwitchActionMap;

        Debug.Log("Cambio a Inverted Map");
    }

    InputActionMap placeHolderActionMap;
    private void MenuControl(InputAction.CallbackContext context)
    {
        if (canvasObj == null)
            return;

        if (placeHolderActionMap == null)
            placeHolderActionMap = playerInput.currentActionMap;

        if (!canvasObj.activeSelf)
        {
            canvasObj.SetActive(true);
            playerInput.SwitchCurrentActionMap(mapUI.name);
        }
        else
        {
            canvasObj.SetActive(false);
            playerInput.SwitchCurrentActionMap(placeHolderActionMap.name);
            placeHolderActionMap = null;
        }
    }

    private void ActivatePlayerBasic()
    {
        playerInput.SwitchCurrentActionMap("PlayerBasic");

        DesSuscribeActions();

        moveAction = playerBasic.FindAction("Move");
        switchAction = playerBasic.FindAction("SwitchMap");

        switchAction.performed += SwitchActionMap;

        Debug.Log("Cambio a Basic Map");
    }

    private void DesSuscribeActions()
    {
        if (moveAction != null)
            moveAction.performed -= AttackExample;
        if (switchAction != null)
            switchAction.performed -= SwitchActionMap;
    }

    private void OnDisable()
    {
        attackAction.performed -= AttackExample;
        attackAction.canceled -= StopAttackExample;
        switchAction.performed -= SwitchActionMap;
        if (menuActivateDeactivate != null)
            menuActivateDeactivate.performed -= MenuControl;

        playerBasic.Disable();
    }
}
