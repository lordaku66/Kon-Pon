using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    private CanvasGroup cg;
    public TMP_Text interactableName;
    public SuperTextMesh message;
    public TMP_Text messageCount;

    [SerializeField] private LeanTweenType tweenType = LeanTweenType.linear;
    [SerializeField] private float inDuration = 0.4f;
    [SerializeField] private float outDuration = 0.4f;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        cg = GetComponent<CanvasGroup>();
        interactableName = GetComponentInChildren<TMP_Text>();
        message = GetComponentInChildren<SuperTextMesh>();
        //messageCount = GetComponentInChildren<TMP_Text>();

        FadeIn();

        // Subscribe to CloseDialogue Event
        inputManager.OnDialogueExit += CloseDialogue;
    }

    // Unsubsribe to CloseDialogue Event
    private void OnDisable()
    {
        inputManager.OnDialogueExit -= CloseDialogue;
    }
    private void FadeIn()
    {
        var fadeDelay = 1.2f;
        cg.LeanAlpha(1, inDuration).setDelay(fadeDelay).setEase(tweenType);
    }

    public void CloseDialogue()
    {
        var fadeDelay = 0.2f;
        cg.LeanAlpha(0, outDuration).setDelay(fadeDelay).setEase(tweenType).setDestroyOnComplete(cg.gameObject);
    }

    public void SetName(string newName)
    {
        interactableName.text = newName;
    }

    public void SetMessage(string newMessage)
    {
        message.text = newMessage;
    }

    public void GetMessageCount(string messageIndex, string totalCount)
    {
        messageCount.text = messageIndex + "/" + totalCount;
    }
}