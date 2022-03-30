using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FetchQuest
{
    [Header("Quest Name, Quest Giver & Deliverable")]
    public string questName = "Insert Quest Name Here";
    public InteractableObject questGiver;
    public GameObject fetchItem;

    [Header("Indicator Sprite (Set By Quest Manager)")]
    [SerializeField] private GameObject indicator = null;

    [Header("Custom Indicator Position")]
    public Vector3 indicatorPosition = new Vector3(0, 0.05f, 0);

    [Header("State Booleans")]
    [SerializeField] private bool isActive;
    [SerializeField] private bool isComplete;
    [SerializeField] private bool isVisible;

    public void SetActive(bool activity)
    {
        isActive = activity;
    }

    public void SetComplete()
    {
        isComplete = true;
        isActive = false;
        isVisible = false;
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public void SetVisble(bool visibility)
    {
        isVisible = visibility;
    }

    public bool IsVisible()
    {
        return isVisible;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetIndicatorStatus(GameObject questIndicator)
    {
        indicator = questIndicator;
    }

    public void SetIndicatorIcon(Sprite icon)
    {
        indicator.GetComponent<SpriteRenderer>().sprite = icon;
    }

    public GameObject GetQuestStatus()
    {
        return indicator;
    }

    public string GetQuestName()
    {
        return questName;
    }
}
