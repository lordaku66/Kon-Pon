using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// KonPon 2021
/// 
/// This script processess all events that take place on the game canvasses using some delegate events. 
/// It keeps track of relevant prefab instances and objects that are selected by the player's cursor.
/// 
/// Author: Jacques Visser
/// </summary>

// TODO: Turn off language lables & player movement on Journal open

public class UIManager : MonoBehaviour
{
    public event Action OnUIClick;
    public event Action OnUIDrag;
    public event Action OnUIDrop;
    public event Action OnSignClicked;
    public event Action OnKarutaStart;
    public event Action OnCardSubmit;
    public event Action OnKarutaEnd;
    public event Action OnJournalOpen;
    public event Action OnJournalGoBack;
    public event Action OnJournalClosed;

    [Header("GAME CANAVASSES")]
    public GameObject karutaCanvas;
    public GameObject cameraBorderCanvas;
    public GameObject dialogueboxCanvas;

    [Header("ACTIVE UI INSTANCES")]
    public GameObject selectedKarutaCard;
    public GameObject activeKarutaGame;
    public GameObject labelledItem;
    public GameObject labelPrefab;
    public GameObject journalMenuButton;

    [Header("UI Overlay States")]
    public bool journalActive;
    public bool karutaActive;

    [Header("PRESET PREFAB INSTANCES")]
    public GameObject howToPlayScreen;
    [SerializeField] private List<GameObject> karutaGames;
    [SerializeField] private Transform karutaParent;
    public bool notbookClosing;

    [HideInInspector]
    public LabelBehavior signBehavior;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData clickData;
    private Vector3 lastMousePos;
    private DialogueManager dialogueManager;

    // UI Raycast Results
    List<RaycastResult> clickResults = new List<RaycastResult>();

    [Header("CLICKABLE OBJECTS")]
    [SerializeField] List<GameObject> clickedUIObjects = new List<GameObject>();

    [Header("LABELLED OBJECTS")]
    [SerializeField] LanguageLabels[] allLables;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        allLables = FindObjectsOfType<LanguageLabels>();
    }

    private void OnEnable()
    {
        OnUIClick += DraggableCheck;
        dialogueManager.OnKarutaTrigger += InstantiateKarutaGame;

        OnJournalClosed += EnableLanguageLabels;
        OnKarutaEnd += EnableLanguageLabels;

        graphicRaycaster = karutaCanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);

        if (labelPrefab != null)
        {
            if (labelPrefab.GetComponent<LabelBehavior>())
            {
                signBehavior = labelPrefab.GetComponent<LabelBehavior>();
            }
            else
            {
                Debug.LogWarning("Label Prefab is either not assigned in the inspector or is missing a LabelBehavior component");
            }
        }
    }

    public void InstantiateKarutaGame(int karutaGameIndex)
    {
        activeKarutaGame = Instantiate(karutaGames[karutaGameIndex], karutaParent);
        OnKarutaStart?.Invoke();
        karutaActive = true;
    }

    public void DraggableCheck()
    {
        clickData.position = Input.mousePosition;
        clickResults.Clear();
        graphicRaycaster.Raycast(clickData, clickResults);

        clickedUIObjects.Clear();
        foreach (RaycastResult item in clickResults)
        {
            if (item.gameObject.tag == "Draggable")
            {
                clickedUIObjects.Add(item.gameObject);
            }
        }

        if (clickedUIObjects.Count > 0)
        {
            SetDraggableObject();
        }
        //else{Debug.Log("No Draggable Objects Selected.");}
    }

    private void SetDraggableObject()
    {
        selectedKarutaCard = clickedUIObjects[0];
        OnUIDrag?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUIClick?.Invoke();
        }

        if (labelledItem != null && Input.GetMouseButtonDown(0))
        {
            OnSignClicked?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedKarutaCard != null)
            {
                OnUIDrop?.Invoke();

                if (GetSelectedDraggable().submitted)
                {
                    OnCardSubmit?.Invoke();
                    GetSelectedDraggable().submitted = false;
                }
            }
        }

        if (notbookClosing)
        {
            OnJournalClosed?.Invoke();
            notbookClosing = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !journalActive && !karutaActive)
        {
            OnJournalOpen?.Invoke();
            journalActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && journalActive && !karutaActive)
        {
            OnJournalGoBack?.Invoke();
            journalActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !journalActive && karutaActive)
        {
            OnKarutaEnd?.Invoke();
        }

        if (karutaActive)
        {
            HideJounralButton();
            CursorManager.SetCursor(CursorManager.defaultCursor);
        }

        if (journalActive)
        {
            CursorManager.SetCursor(CursorManager.defaultCursor);
        }

        lastMousePos = Input.mousePosition;
    }

    public KarutaCard GetSelectedDraggable()
    {
        return selectedKarutaCard.GetComponent<KarutaCard>();
    }

    public LanguageLabels GetSelectedSign()
    {
        return labelledItem.gameObject.GetComponent<LanguageLabels>();
    }

    public void SetSelectedSign(GameObject sign)
    {
        labelledItem = sign;
    }

    public void HideJounralButton()
    {
        journalMenuButton.SetActive(false);
    }

    public void EnableLanguageLabels()
    {
        foreach (LanguageLabels label in allLables)
        {
            label.enabled = true;
        }
    }
}
