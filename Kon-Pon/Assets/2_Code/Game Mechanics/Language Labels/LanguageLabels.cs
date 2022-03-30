using System;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This script controls the language cycle for the language label system.
/// 
/// The current trasnlation shown by a label is represented using the SignStates enum below.
/// This script is what determined the "selectedSign" varible in the UI manager based on the OnMouseEnter and Exit methods.
/// 
/// </summary>

[System.Serializable]
public enum SignStates
{
    Romaji,
    Kana,
    English
}

public class LanguageLabels : MonoBehaviour
{
    private UIManager uiManager;
    public event Action OnHover;
    public event Action OnHoverExit;
    [SerializeField] private int timesViewed;
    public string englishWord;
    public string romajiWord;
    public string kanaWord;
    public SignStates language;
    public bool isHighlighted;
    public bool showOnTrigger = true;

    [HideInInspector]
    public bool interacted;

    [SerializeField] private bool isSpecial;

    [SerializeField] private Vector3 specialSpawnPos;

    [SerializeField] private Vector3 specialLocalSize;

    private LabelBehavior signBehavior;

    [SerializeField] private SpriteRenderer shineFX;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        uiManager.OnSignClicked += CycleLanguage;
        uiManager.OnJournalOpen += TurnOff;
        uiManager.OnKarutaStart += TurnOff;
    }

    private void OnDisable()
    {
        uiManager.OnSignClicked -= CycleLanguage;
        uiManager.OnJournalOpen -= TurnOff;
        uiManager.OnKarutaStart -= TurnOff;
    }

    private void OnMouseEnter()
    {
        if (this.enabled)
        {
            CursorManager.SetCursor(CursorManager.inspectCursor);

            isHighlighted = true;
            uiManager.SetSelectedSign(Instantiate(uiManager.labelPrefab, transform));

            uiManager.labelledItem.name = (name + "'s Label");

            // Preset Label Position
            if (!isSpecial)
            {
                uiManager.labelledItem.transform.localPosition = new Vector3(0, 0.2f, 0);
                uiManager.labelledItem.GetComponent<LabelBehavior>().SetWordStrings();
            }
            else // Special Label Position
            {
                uiManager.labelledItem.transform.localPosition = specialSpawnPos;
                uiManager.labelledItem.transform.localScale = specialLocalSize;
                uiManager.labelledItem.GetComponent<LabelBehavior>().SetWordStrings();
            }

            if (shineFX.enabled)
            {
                shineFX.gameObject.LeanAlpha(0, 0.25f);
            }
        }
    }

    private void OnMouseExit()
    {
        CursorManager.SetCursor(CursorManager.defaultCursor);

        isHighlighted = false;
        OnHoverExit?.Invoke();

        if (uiManager.labelledItem != null)
        {
            signBehavior.GoAway();
            timesViewed++;
        }
    }

    private void OnMouseOver()
    {
        OnHover?.Invoke();
        signBehavior = GetComponentInChildren<LabelBehavior>();
    }

    public void CycleLanguage()
    {
        if (isHighlighted)
        {
            if (language == SignStates.Kana)
            {
                language = SignStates.English;
            }
            else if (language == SignStates.Romaji)
            {
                language = SignStates.Kana;
            }
            else if (language == SignStates.English)
            {
                language = SignStates.Romaji;
            }
        }

        interacted = true;
    }

    public int GetTimesViewed()
    {
        return timesViewed;
    }

    public void TurnOff()
    {
        Destroy(uiManager.labelledItem);
        uiManager.labelledItem = null;
        Debug.Log("I Sleep");
        this.enabled = false;
    }
}
