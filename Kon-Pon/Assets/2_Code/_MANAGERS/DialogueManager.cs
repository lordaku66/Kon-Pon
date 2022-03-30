using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// The Dialogue Manager controls the following:
/// 
/// - Instancing dialogue boxes when the Player interacts with an interactable.
/// - Reading and Displaying Dialogue from the JSON conversation files
/// - Signaling the Quest Manager to begin a quest from a line of dialogue. 
/// 
/// Author: Jacques Visser
/// </summary>

public class DialogueManager : MonoBehaviour
{
    // Manager Dependencies
    private InputManager inputManager;
    private UIManager uiManager;
    private FetchQuestManager questManager;

    // Quest Trigger Event
    public event Action OnQuestTrigger;

    // Karuta Trigger Event
    public event Action<int> OnKarutaTrigger;

    // JSON Dialogue File & Dialogue Prefabs
    [Header("JSON Goes Here")]
    [SerializeField] private TextAsset JSONFile;
    [SerializeField] private TextAsset questFeedback;

    [Header("Response Prefabs & State")]
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject midQuestDialogue;
    [SerializeField] private GameObject endQuestDialogue;
    [SerializeField] private GameObject invalidItemDialogue;
    [SerializeField] private Transform boxParent;
    public bool isDialogueActive;



    // Static reference to the Dialogue arrays which store an NPC's dialogie serialized from JSON
    static DialogueCollection dialogueCollection;
    static ResponseCollection responseCollection;

    // The dialogue collection represented in a list data structure
    List<Dialogue[]> dialogueList = new List<Dialogue[]>();

    // The active dialogue box
    private GameObject dialogueInstance;

    // The DialogueBox Component attached to the dialogue instance.
    private DialogueBox boxProperties;

    private InteractableObject selectedObject;
    private int NPC_ID;

    int msgCount, dialogueCount;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        questManager = FindObjectOfType<FetchQuestManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        inputManager.OnInteractableClicked += ShowDialogueBox;
        inputManager.OnDialogueAdvanced += AdvanceDialogue;
        inputManager.OnDialogueExit += DialogueExit;

        questManager.OnBadItem += ItemIsInvalid;

        dialogueCollection = JsonUtility.FromJson<DialogueCollection>(JSONFile.text);
        responseCollection = JsonUtility.FromJson<ResponseCollection>(questFeedback.text);

        dialogueList.Add(dialogueCollection.TEMPLATE);
        dialogueList.Add(dialogueCollection.test_npc);
        dialogueList.Add(dialogueCollection.karuta);
        dialogueList.Add(dialogueCollection.mush);

        dialogueList.Add(responseCollection.midquest);
        dialogueList.Add(responseCollection.postquest);
    }

    // Shows a dialogue box and displays the name of the object interacted with. 
    public void ShowDialogueBox()
    {
        if (!isDialogueActive)
        {
            selectedObject = inputManager.GetSelectedObject();
            NPC_ID = selectedObject.properties.GetID();

            if (questManager.GetActiveQuestID() == -1)
            {
                if (selectedObject.properties.IsQuestTurnedIn())
                {
                    InstanceDialogue(dialoguePrefab);
                    boxProperties.SetMessage(JSONString(dialogueList[5]));
                    selectedObject.properties.TurnIn(false);
                }
                else
                {
                    InstanceDialogue(dialoguePrefab);

                    // Set the display messsage to be a line of dialogue from the selected NPC's Object ID.
                    boxProperties.SetMessage(JSONString(dialogueList[NPC_ID]));
                    dialogueCount = dialogueList[NPC_ID].Length;
                }
            }
            else if (questManager.GetActiveQuest().IsActive())
            {
                if (questManager.itemIsBad)
                {
                    InstanceDialogue(invalidItemDialogue);
                }
                else
                {
                    InstanceDialogue(dialoguePrefab);
                    boxProperties.SetMessage(JSONString(dialogueList[4]));
                }

            }

            // Call the selected objects OnInteract function
            selectedObject.properties.OnInteract();

            // Set the display name in the dialogue box to the name of the NPC
            boxProperties.SetName(selectedObject.properties.GetName());

            boxProperties.interactableName.color = selectedObject.properties.GetColor();

            msgCount = selectedObject.properties.GetMessageIndex() + 1;
            boxProperties.GetMessageCount(msgCount.ToString(), dialogueCount.ToString());
        }
        else
        {
            Debug.LogWarning("Dialogue Manager: There is already dialogue box active; Two dialogue boxes cannot be active at once.");
        }
    }

    public void ItemIsInvalid()
    {
        InstanceDialogue(dialogueInstance);
    }

    public string JSONString(Dialogue[] conversation)
    {
        selectedObject = inputManager.GetSelectedObject();
        NPC_ID = selectedObject.properties.GetID();

        string messageString = "";

        // Check the message index doesn't exceed dialogue length
        if (selectedObject.properties.GetMessageIndex() > conversation.Length - 1)
        {
            selectedObject.properties.SetMessageIndex(0);
        }

        foreach (Dialogue item in conversation)
        {
            // Check if the ID of the current message & the NPC's message index match
            if (item.id == selectedObject.properties.GetMessageIndex())
            {
                if (item.questTrigger)
                {
                    OnQuestTrigger?.Invoke();
                }

                if (item.newSpeaker != "")
                {
                    boxProperties.SetName(item.newSpeaker);
                }

                if (item.karutaGame != -1)
                {
                    OnKarutaTrigger?.Invoke(item.karutaGame);
                }

                // Check the NPC's conversation index is less than the conversation length
                if (selectedObject.properties.GetMessageIndex() < conversation.Length)
                {
                    messageString = item.message;
                }
            }
        }
        return messageString;
    }

    private void Update()
    {
        if (boxParent.childCount >= 1)
        {
            isDialogueActive = true;
        }
        else
        { isDialogueActive = false; }

    }

    public void AdvanceDialogue()
    {

        selectedObject = inputManager.GetSelectedObject();
        NPC_ID = selectedObject.properties.GetID();

        if (isDialogueActive)
        {
            selectedObject.properties.OnDialogueAdvanced();
            msgCount = selectedObject.properties.GetMessageIndex() + 1;
            boxProperties.SetMessage(JSONString(dialogueList[NPC_ID]));

            if (msgCount > dialogueCount)
            {
                boxProperties.GetMessageCount("1", dialogueCount.ToString());
            }
            else
            {
                boxProperties.GetMessageCount(msgCount.ToString(), dialogueCount.ToString());
            }

        }
    }

    // Instantiates one of the prefabs as 
    public void InstanceDialogue(GameObject prefab)
    {
        //Destroy(dialogueInstance);
        dialogueInstance = Instantiate(prefab, boxParent);
        boxProperties = dialogueInstance.GetComponent<DialogueBox>();
    }

    public DialogueBox GetMessage()
    {
        return boxProperties;
    }

    private void DialogueExit()
    {
        isDialogueActive = false;
    }

    public int GetDialogueCount()
    {
        return dialogueCount;
    }

    public int GetNPC()
    {
        return NPC_ID;
    }

    public int GetIndex()
    {
        return msgCount;
    }
}
