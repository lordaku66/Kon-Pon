using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// The quest manager controls the states of the quests created using the structure defined
/// in 'FetchQuest.cs'. 
/// This manager also controls the states of quest inicators show if a quest is active / inactive
/// Finally, this manager validates pick up items when the player attempts to return them to an NPC.
/// 
/// OnQuestAccept, is triggered from the dialogue manager and sets the relative quest to an active state.
/// When a quest is active no other quests can be accessed, and the quests indicator
///  
/// OnQuestTurn, in is triggered when the player speaks with the relavent quest giver while holding 
/// a quest item to turn-in and validates the turn-in item.
/// 
/// OnBadItem is triggered if the player is not holding the right item and signals the Dialogue
/// Manager to instantiate the according response from the NPC. 
/// 
/// The active quest is set from the DialogueManager but the ctate changes for a quest are processed
/// here.
///  
/// /// Author: Jacques Visser
/// </summary>

// TODO: Make the fetch item a prefab instance from the item stack instead of just a Game Object

public class FetchQuestManager : MonoBehaviour
{
    public event Action OnQuestAccept;
    public event Action OnQuestTurnIn;
    public event Action OnBadItem;

    [HideInInspector]
    [SerializeField] private FetchQuest activeQuest;
    [SerializeField] private string activeQuestName;
    [SerializeField] private int activeQuestID;
    [SerializeField] private List<FetchQuest> KonPonQuests = new List<FetchQuest>();

    [Header("Indicator Sprites")]
    [SerializeField] private Sprite exclamationIndicator;
    [SerializeField] private Sprite questionIndicator;
    public Sprite turnInIndicator;
    private InputManager inputManager;
    private DialogueManager dialogueManager;

    [Header("The Item Held By Player")]
    public GameObject heldItem;
    public Liftable heldItemProperites;
    public bool itemIsBad;

    [Header("Particle Effects")]
    [SerializeField] private GameObject questFireworks;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnEnable()
    {
        GameObject questStatus;
        SpriteRenderer questIndicator;
        InteractableObject NPC;

        foreach (FetchQuest quests in KonPonQuests)
        {
            NPC = quests.questGiver;
            questStatus = new GameObject();
            questStatus.name = "Quest Indicator";
            questStatus.transform.parent = quests.questGiver.transform;
            questStatus.transform.localPosition = quests.indicatorPosition;
            questStatus.transform.localScale = Vector3.one / 2;

            quests.SetIndicatorStatus(questStatus);

            questIndicator = questStatus.AddComponent<SpriteRenderer>();
            questIndicator.sprite = exclamationIndicator;
            questIndicator.gameObject.LeanMoveLocalY(quests.indicatorPosition.y + 0.05f, 0.5f).setLoopPingPong();

            questStatus.SetActive(quests.IsVisible());
        }

        activeQuestID = -1;

        inputManager.OnInteractableClicked += ValidateHeldItem;
        dialogueManager.OnQuestTrigger += SetActiveQuest;

        inputManager.OnPickup += ItemPickedUp;
        inputManager.OnDrop += ItemDroppped;

        OnQuestTurnIn += TurnIn;
        OnQuestTurnIn += PlayFireworks;
    }

    // Show & Hide Quests
    private void Update()
    {
        foreach (FetchQuest quest in KonPonQuests)
        {
            if (!quest.IsVisible() && !quest.IsActive())
            {
                quest.GetQuestStatus().GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            else { quest.GetQuestStatus().GetComponentInChildren<SpriteRenderer>().enabled = true; }

            if (activeQuestID != -1)
            {
                quest.questGiver.properties.TurnIn(quest.IsComplete());
                quest.questGiver.properties.SetQuestActive(quest.IsActive());
            }


            // quest.GetQuestStatus().transform.localPosition = quest.indicatorPosition;

        }

        if (activeQuestID != -1)
        {
            for (int i = 0; i < KonPonQuests.Count; i++)
            {
                KonPonQuests[i].SetVisble(false);
            }
        }
    }

    public int GetActiveQuestID()
    {
        return activeQuestID;
    }

    public FetchQuest GetActiveQuest()
    {
        return activeQuest;
    }

    public void SetActiveQuest()
    {
        int questID = inputManager.GetSelectedObject().properties.GetID() - 1;


        if (!KonPonQuests[questID].IsComplete())
        {
            Sprite itemSprite = KonPonQuests[questID].fetchItem.
            GetComponentInChildren<SpriteRenderer>().sprite;

            SpriteRenderer indicator = KonPonQuests[questID].GetQuestStatus()
            .GetComponentInChildren<SpriteRenderer>();

            KonPonQuests[questID].SetActive(true);

            activeQuest = KonPonQuests[questID];
            activeQuestName = KonPonQuests[questID].questName;
            activeQuestID = questID;

            indicator.sprite = questionIndicator;
            indicator.gameObject.transform.localScale = Vector3.one / 2;

            OnQuestAccept?.Invoke();
        }
    }

    public void ItemPickedUp()
    {
        activeQuest.SetIndicatorIcon(turnInIndicator);
    }
    public void ItemDroppped()
    {
        activeQuest.SetIndicatorIcon(questionIndicator);
    }

    public void ValidateHeldItem()
    {
        foreach (FetchQuest quest in KonPonQuests)
        {
            if (quest.IsActive() & heldItem != null)
            {
                if (heldItemProperites.turnInNPC == quest.questGiver)
                {
                    if (quest.questGiver == inputManager.GetSelectedObject())
                    {
                        Debug.Log("Quest Manager: " + quest.questName + " turned in");
                        KonPonQuests[activeQuestID].SetComplete();

                        OnQuestTurnIn?.Invoke();
                    }
                }
                else
                {
                    Debug.Log("Quest Manager: Incorrect Item");
                    itemIsBad = true;
                }
            }
        }
    }

    public void TurnIn()
    {
        KonPonQuests[activeQuestID].SetComplete();
        inputManager.GetSelectedObject().properties.TurnIn(true);
        activeQuest = null;
        activeQuestID = -1;

        // TODO: Instance Particle Effect

        DropItem();
    }

    public void DropItem()
    {
        float dropTime = 0.8f;
        heldItem.LeanAlpha(0, dropTime).setDelay(0.2f).setDestroyOnComplete(heldItem);
        heldItem.LeanMoveLocalY(-0.25f, dropTime).setEase(LeanTweenType.easeInSine);
        heldItem = null;
        heldItemProperites = null;
    }
    public void PlayFireworks()
    {
        GameObject fireworks = Instantiate(questFireworks, inputManager.GetSelectedObject().gameObject.transform);
        fireworks.transform.localPosition = new Vector3(fireworks.transform.localPosition.x, 0.2f, 0.5f);
        fireworks.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
    }
}