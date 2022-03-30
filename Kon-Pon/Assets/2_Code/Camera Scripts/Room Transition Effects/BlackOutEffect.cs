using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This script creates a fade in and out effect when the player needs to teleport.
/// It works along with the FastTravel script.
/// 
/// Author: Krishna Thiruvengadam 
/// </summary>
public class BlackOutEffect : MonoBehaviour
{
    public float fadeSpeed = 2f;
    CanvasGroup canvasGroup;
    FastTravel checkTeleport;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        checkTeleport = FindObjectOfType<FastTravel>();
    }

    private void OnEnable() {
        FastTravel.OnTeleport += FadeIn;
        FastTravel.OnTeleported += FadeOut;
    }

    public void FadeIn()
    {
        canvasGroup.LeanAlpha(1, fadeSpeed);
    }

    public void test()
    {
        checkTeleport.TeleportPlayer();
    }

    public void FadeOut()
    {
        canvasGroup.LeanAlpha(0, fadeSpeed).setDelay(1f);
    }
}
