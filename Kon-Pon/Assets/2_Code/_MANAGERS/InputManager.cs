using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// KonPon 2021
/// 
/// This script processes input for KonPon using delegate events to call multiple methods from different scripts
/// using the observer pattern.
/// 
/// Events such as OnClick & 
/// 
/// Author: Jacques Visser and Krishna Thiruvengadam
/// </summary>

public class InputManager : MonoBehaviour
{
    // Event Delegates
    public event Action OnClick;
    public event Action OnInteractableClicked;
    public event Action OnDialogueExit;
    public event Action OnDialogueAdvanced;
    public event Action OnPickup;
    public event Action OnDrop;

    // Store a Unity layer to return data from in a raycast.
    [SerializeField] private LayerMask whatIsInteractable;

    // Store raycast max distance
    [SerializeField] private int raycastDistance = 10;

    // Store the interactable object that was last clicked on.
    public GameObject selectedInteractable;
    public GameObject currentItemStack;

    public Func<bool> OnPickupAttempt;
    public bool canPickup = false;

    // Reference to other classes
    Player player;
    DialogueManager dialogueManager;
    UIManager uiManager;
    SuperTextMesh currentDialogue;

    float timer = 0f;
    [SerializeField] float dialogueDelay = 0.5f;
    [SerializeField] float dialogueExitDelay = 1.2f;
    private bool dialogueExited = false;

    int selectedObjIndex, dialogueCount;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        OnClick += InteractableCheck;

        uiManager.OnJournalOpen += DisablePlayerInput;
        uiManager.OnKarutaStart += DisablePlayerInput;

        uiManager.OnJournalClosed += EnablePlayerInput;
        uiManager.OnKarutaEnd += EnablePlayerInput;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void DisablePlayerInput()
    {
        player.enabled = false;
    }

    public void EnablePlayerInput()
    {
        player.enabled = true;
    }

    public void InteractableCheck()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        selectedInteractable = null;

        if (Physics.Raycast(ray, out hit, raycastDistance, whatIsInteractable))
        {
            // Debug.Log(hit.transform.name + " : Interactable");
            selectedInteractable = hit.transform.gameObject;

            if (selectedInteractable != null)
            {
                OnInteractableClicked?.Invoke();
            }
        }
        else if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Debug.Log(hit.transform.name + " : Uninteractable");

            OnDialogueExit.Invoke();
        }
    }

    public InteractableObject GetSelectedObject()
    {
        return selectedInteractable.GetComponent<InteractableObject>();
    }

    // Poll for mouse input & invoke the click delegate
    private void Update()
    {
        selectedObjIndex = dialogueManager.GetIndex();
        dialogueCount = dialogueManager.GetDialogueCount();
        timer += Time.deltaTime;

        //fastTravel = currentTeleporter.GetComponent<FastTravel>();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedInteractable != null)
            {
                currentDialogue = dialogueManager.GetMessage().message;

                if (currentDialogue.reading == true)
                {
                    timer = 0f;
                    currentDialogue.reading = false;
                }
                else
                {
                    if (timer >= dialogueDelay)
                    {
                        OnDialogueAdvanced?.Invoke();
                        timer = 0f;
                    }
                }
            }
            else
            {
                if (!dialogueExited)
                {
                    OnClick?.Invoke();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            timer = 0f;
            OnDialogueExit?.Invoke();
            dialogueExited = true;
            selectedInteractable = null;
        }

        if (timer >= dialogueExitDelay && dialogueExited == true)
        {
            dialogueExited = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItemStack != null && !GetPlayer().isHoldingItem)
            {
                Debug.Log("OnPickUp");
                OnPickup?.Invoke();
            }

            else if (GetPlayer().isHoldingItem)
            {
                Debug.Log("OnDrop");
                OnDrop?.Invoke();

            }
        }
    }

    public bool DialogueExit()
    {
        return dialogueExited;
    }
}