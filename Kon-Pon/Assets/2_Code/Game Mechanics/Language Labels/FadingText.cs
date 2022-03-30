using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private void OnEnable()
    {
        LeanTween.alpha(gameObject, 1, 0.5f);
    }

    private void OnDisable()
    {
        LeanTween.alpha(gameObject, 0, 0.5f);
    }
}
