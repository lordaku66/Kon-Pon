using UnityEngine;
using UnityEngine.UI;

public class HowToPlayWindow : MonoBehaviour
{
    [Header("Animation Properties")]
    [SerializeField] private float inTime = 0;
    [SerializeField] private float outTime = 0;
    [SerializeField] private LeanTweenType easeType;

    [Header("Size Deltas")]
    [SerializeField] private Vector3 startingSize = new Vector3();
    [SerializeField] private Vector3 endSize = Vector3.one;

    private void OnEnable()
    {
        GetComponent<RectTransform>().LeanScale(endSize, inTime).setEase(easeType);
    }

    public void OnClose()
    {
        GetComponent<RectTransform>().LeanScale(startingSize, outTime).setOnComplete(OnDisable);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
