using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This class stores some important data about a single interactable object in KonPon.
/// 
/// ObjectID - used to read data from JSON files.
/// ObjectName - used to show the objects name in a dialogue box.
/// MesssageIndex - used as a pointer to the current line of dialogue the object should be broadcasting.
/// TimesInteracted - used to keep track how many timmes this npc was interacted with. 
/// 
/// Author: Jacques Visser
/// </summary>

[System.Serializable]
public class Interactable
{
    [Header("NPC Dialogue Properties")]
    [SerializeField] private int objectID;
    [SerializeField] private string objectName;
    [SerializeField] private Color nameColor = new Color(1, 1, 1, 1);
    [SerializeField] private int messageIndex = 0;
    [SerializeField] private int timesInteracted = 0;

    [Header("Quest State - Set in FetchQuestManager")]
    [SerializeField] private bool isQuestGiven;
    [SerializeField] private bool isQuestTurnedIn;


    public string GetName()
    {
        return objectName;
    }

    public int GetID()
    {
        return objectID;
    }

    public void OnInteract()
    {
        timesInteracted++;
    }

    public void OnDialogueAdvanced()
    {
        messageIndex++;
    }

    public void SetMessageIndex(int id)
    {
        messageIndex = id;
    }

    public int GetMessageIndex()
    {
        return messageIndex;
    }

    public bool IsQuestGiven()
    {
        return isQuestGiven;
    }

    public void SetQuestActive(bool isActive)
    {
        isQuestGiven = isActive;
    }

    public bool IsQuestTurnedIn()
    {
        return isQuestTurnedIn;
    }

    public void TurnIn(bool completeState)
    {
        isQuestTurnedIn = completeState;
    }

    public Color GetColor()
    {
        return nameColor;
    }
}
