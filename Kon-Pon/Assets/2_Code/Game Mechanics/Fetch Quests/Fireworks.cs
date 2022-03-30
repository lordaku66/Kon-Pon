using UnityEngine;

/// <summary>
///  KonPon 2021
/// 
/// This script controls the behavior for firework particel effects so they live for
/// a few secondds before the particle effect is destroyed.
/// 
/// Author: Jacques Visser
/// </summary>

public class Fireworks : MonoBehaviour
{
    public ParticleSystem FX;
    [SerializeField] private const float countDownTime = 6f;
    public float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= countDownTime)
        {
            OnDisable();
        }

    }

    private void OnDisable()
    {
        this.enabled = false;
    }
}
