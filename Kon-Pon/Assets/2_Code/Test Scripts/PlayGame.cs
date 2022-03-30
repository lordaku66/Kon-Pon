using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
    private InputManager inputManager;
    private CanvasGroup canvasGroup;

    private float fadeTime = 0.5f;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        inputManager.OnClick += StartGame;
    }

    private void OnDisable()
    {
        inputManager.OnClick -= StartGame;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        canvasGroup.LeanAlpha(0, fadeTime);
    }
}
