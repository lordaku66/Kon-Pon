using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// KonPon 2021
/// </summary>

public class CinematicBars : MonoBehaviour
{
    [SerializeField] private float animSpeed = 0.5f;
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;
    private InputManager inputManager;

    [SerializeField] private float topStartPos;
    [SerializeField] private float botStartPos;

    [SerializeField] private float moveDistance;

    private void Awake()
    {
        topStartPos = topBar.localPosition.y;
        botStartPos = bottomBar.localPosition.y;
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        inputManager.OnInteractableClicked += ShowBars;
        inputManager.OnDialogueExit += HideBars;
    }

    private void ShowBars()
    {
        topBar.LeanMoveLocalY(topStartPos - moveDistance, animSpeed);
        bottomBar.LeanMoveLocalY(botStartPos + moveDistance, animSpeed);
    }

    private void HideBars()
    {
        topBar.LeanMoveLocalY(topStartPos, animSpeed);
        bottomBar.LeanMoveLocalY(botStartPos, animSpeed);
    }

    private void OnDisable()
    {
        inputManager.OnInteractableClicked -= ShowBars;
        inputManager.OnDialogueExit -= HideBars;
    }
}
