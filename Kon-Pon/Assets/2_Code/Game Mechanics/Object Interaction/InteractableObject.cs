using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This script fires a raycast when the mouse is clicked to check if an object
/// is interactable. The mouse click is detected by the Input Manager.
/// 
/// Author: Jacques Visser
/// </summary>

public class InteractableObject : MonoBehaviour
{
    // Interactable objects properties are instantated and exposed in the inspector for me to edit 
    // in Unity.
    public Interactable properties = new Interactable();
    private DialogueManager dialogueManager;
    private FetchQuestManager questManager;
    private Outline highlight;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        questManager = FindObjectOfType<FetchQuestManager>();
    }

    private void OnEnable()
    {
        dialogueManager.OnQuestTrigger += SetQuestActive;
        //questManager.OnQuestTurnIn += PlayFireworks;
    }

    private void OnMouseEnter()
    {
        CursorManager.SetCursor(CursorManager.talkCursor);
    }

    private void OnMouseOver()
    {
        if (GetComponent<Outline>())
        {
            highlight.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (GetComponent<Outline>())
        {
            highlight.enabled = false;
        }

        CursorManager.SetCursor(CursorManager.defaultCursor);
    }

    public void SetQuestActive()
    {
        properties.SetQuestActive(true);
    }

    public void SetQuestInactive()
    {
        properties.SetQuestActive(false);
    }
}
