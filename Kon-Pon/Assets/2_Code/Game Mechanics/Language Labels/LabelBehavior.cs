using UnityEngine;
using TMPro;

public class LabelBehavior : MonoBehaviour
{
    public GameObject kana;
    private SuperTextMesh kanaWord;

    public GameObject romaji;
    private SuperTextMesh romajiWord;

    public GameObject english;
    private SuperTextMesh englishWord;

    // [Header("First Viewed Icons")]
    // [SerializeField] private GameObject viewedIcon;
    // [SerializeField] private GameObject newShineFX;

    [Header("Animation Properties")]
    public float fadeInDelay = 0.2f;
    public float fadingTime = 0.4f;
    private UIManager uiManager;

    private LanguageLabels label;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();

        kanaWord = kana.GetComponent<SuperTextMesh>();
        romajiWord = romaji.GetComponent<SuperTextMesh>();
        englishWord = english.GetComponent<SuperTextMesh>();

        label = GetComponentInParent<LanguageLabels>();
    }

    private void OnEnable()
    {
        uiManager.OnSignClicked += OnClick;
    }

    public void GoAway()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        uiManager.OnSignClicked -= OnClick;
    }
    public void SetWordStrings()
    {
        kanaWord.text = label.kanaWord;
        romajiWord.text = label.romajiWord;
        englishWord.text = label.englishWord;
    }

    public void OnClick()
    {
        if (label.language == SignStates.Kana)
        {
            kana.SetActive(true);
            romaji.SetActive(false);
            english.SetActive(false);
        }
        else if (label.language == SignStates.Romaji)
        {
            kana.SetActive(false);
            romaji.SetActive(true);
            english.SetActive(false);
        }
        else if (label.language == SignStates.English)
        {
            english.SetActive(true);
            kana.SetActive(false);
            romaji.SetActive(false);
        }
    }

    public void FadeText(SuperTextMesh text, float from, float to, float time)
    {
        Color visible = new Color(1, 1, 1, 1f);
        Color invisible = new Color(1, 1, 1, 0);

        float fade = Mathf.Lerp(invisible.a, visible.a, time);
        text.color = new Color32(255, 255, 255, (byte)(fade * 255));
    }
}
