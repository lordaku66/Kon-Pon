using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// KonPon 2021
///  
/// Controls the game state for one instance of a Karuta Game. 
/// 
/// This script holds one karuta game's parameters in the form of:
/// - How many cards are in the game. 
/// - The game ID.
/// - The sprites for the cards that will be shown.
/// - The names of the cards.
/// - The series text phrases that appear and their order of appearance.
/// 
/// A Karuta Game is instantiated from the UI Manager in the form of a GameObject prefab with these
/// paramters preset in the inspector. The game intances parent canvas is the 'Karuta Canvas'
/// stored in the UI Manager. 
/// 
/// The delegate actions OnCardValid, OnCardInValid and OnPhraseShown are used to register and
/// validate a players reposnse, change the message in the prompt box and signal PhraseBoxBehavior.cs
/// to change the color of the prompt box.
/// 
/// The messages in the prommpt Box are set in this script by reading data from the JSON file
/// based on the games ID, it knows which set of phrases to read from KarutaPhrases.cs
/// 
/// Author: Jacques Visser
/// </summary>

public class KarutaGame : MonoBehaviour
{
    public event Action OnCardValid;
    public event Action OnCardInvalid;
    public event Action OnPhraseShown;


    private Journal journal;

    [Header("Karuta Game Settings")]
    [SerializeField] private int gameID;
    [SerializeField] private int phraseIndex;
    public GameObject cardPrefab;
    [SerializeField] private int gameCards = 1;
    [SerializeField] private string expectedAnswer;
    private CanvasGroup canvasGroup;
    private Transform playArea;
    private SuperTextMesh promptMessage;
    private UIManager uiManager;

    [Header("Karuta Card Images & Names")]
    [SerializeField] Sprite[] cardImages;
    [SerializeField] List<string> cardNames;

    [Header("Animation Properties")]
    [SerializeField] private float fadingTime = 0.5f;
    private Dictionary<int, string> phraseDictionary = new Dictionary<int, string>();

    [Header("Game Phrases JSON")]
    [SerializeField] private TextAsset phrasesJSON;
    static Phrases gamePhrases;
    private List<KarutaPhrase[]> phraseCollection = new List<KarutaPhrase[]>();

    [Header("Tutorial")]
    [SerializeField] private float timer;
    [SerializeField] private TMP_Text tutorialMessage;
    private CanvasGroup tutorialColor;
    private const float maxTime = 8f;

    // The Y position of cards on game start
    private float yOffset
    {
        get
        {
            return -110f;
        }
    }
    // The X position of cards on game start
    private float xRange
    {
        get
        {
            return UnityEngine.Random.Range(-425f, 425f);
        }
    }

    // Instantiates the all the cards in the game, sets their sprites & sets their names. 
    public void SpawnThisManyCards()
    {
        for (int i = 0; i < gameCards; i++)
        {
            // Card Instance.
            var card = Instantiate(cardPrefab, playArea);

            // Set Spawn Position.
            card.transform.localPosition = new Vector3(xRange, yOffset, 0);

            // Set the name of the card.
            card.name = cardNames[i];

            // Set the cards image.
            card.GetComponent<Image>().sprite = cardImages[i];
        }
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();

        journal = FindObjectOfType<Journal>();

        playArea = GameObject.FindGameObjectWithTag("Drop Zone").transform.parent;

        promptMessage = GameObject.FindGameObjectWithTag("Karuta Phrase")
        .GetComponent<SuperTextMesh>();

        canvasGroup = GetComponent<CanvasGroup>();

        tutorialColor = tutorialMessage.gameObject.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        cardNames.Capacity = gameCards;
        expectedAnswer = cardNames[0];

        ReadPhrasesFromJSON();
    }

    private void OnEnable()
    {
        uiManager.OnKarutaStart += StartGame;
        uiManager.OnKarutaEnd += EndGame;
        uiManager.OnCardSubmit += ValidateCard;
        canvasGroup.LeanAlpha(1, fadingTime);
    }

    private void OnDisable()
    {
        uiManager.OnKarutaStart -= StartGame;
        uiManager.OnKarutaEnd -= EndGame;
        uiManager.OnCardSubmit -= ValidateCard;
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (cardNames.Count != gameCards)
        {
            Debug.
            LogError("Karuta Game Manager: Card Name Mismatch, no cards were spawned");
        }
        else if (cardImages.Length != gameCards)
        {
            Debug.
            LogError("Karuta Game Manager: Card Image Count Mismatch, no cards were spawned");
        }
        else
        {
            SpawnThisManyCards();
            promptMessage.text = "<c=FDAB57>Which One Of These Cards Best Matches This Japanese Phrase?</c>";
            StartCoroutine(DisplayStartingText());
        }
    }

    // We use update to as a timer 
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            timer += Time.deltaTime;

            if (timer >= maxTime)
            {
                tutorialMessage.gameObject.SetActive(true);
            }
        }
        else
        {
            timer = 0;
            tutorialMessage.gameObject.SetActive(false);
        }
    }

    public void EndGame()
    {
        Debug.Log("cORN");
        GetComponent<CanvasGroup>().LeanAlpha(0, fadingTime).setOnComplete(OnDisable);
        uiManager.karutaActive = false;
    }

    public void ReadPhrasesFromJSON()
    {
        gamePhrases = JsonUtility.FromJson<Phrases>(phrasesJSON.text);

        phraseCollection.Add(gamePhrases.template);
        phraseCollection.Add(gamePhrases.first);
    }

    public string ShowPhrase(KarutaPhrase[] karutaGame)
    {
        string phraseString = "";

        foreach (KarutaPhrase phrase in karutaGame)
        {
            if (phrase.id == phraseIndex)
            {
                expectedAnswer = phrase.answer;
                phraseString = phrase.kana;
            }
        }
        OnPhraseShown?.Invoke();
        return phraseString;
    }

    public void SwitchPhrases()
    {
        foreach (KarutaPhrase phrase in phraseCollection[gameID])
        {
            if (phrase.id == phraseIndex)
            {
                if (promptMessage.text == phrase.kana)
                {
                    promptMessage.text = phrase.romaji;
                }

                else if (promptMessage.text == phrase.romaji)
                {
                    promptMessage.text = phrase.english;
                }

                else if (promptMessage.text == phrase.english)
                {
                    promptMessage.text = phrase.kana;
                }
            }
        }
    }

    public void ValidateCard()
    {
        if (uiManager.selectedKarutaCard.name == expectedAnswer)
        {

            // Increment phrase index & show next phrase.
            phraseIndex++;

            // Update the text in the phrase box.
            promptMessage.text = "<c=green>Correct!";

            // Lerp Phrase Box Color to Green
            OnCardValid?.Invoke();

            // Animate off-screen & Destroy validted Card.
            StartCoroutine(DisplayTextSlowly(1));

            uiManager.selectedKarutaCard.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f)
            .setDestroyOnComplete(uiManager.selectedKarutaCard);
        }
        else
        {
            // Spit card back out.
            uiManager.selectedKarutaCard.LeanMoveLocal(new Vector3(xRange, yOffset, 0), 0.5f).setEase(LeanTweenType.easeSpring);
            uiManager.GetSelectedDraggable().droppped = false;
            uiManager.GetSelectedDraggable().dropPosition = null;

            // Update the text in the phrase box.
            promptMessage.text = "<c=red>Incorrect!";

            // Lerp Phrase Box Color to Red.
            OnCardInvalid?.Invoke();

            // Fade the text out, change the text and fade back in. 
            StartCoroutine(DisplayTextSlowly(1));
        }
    }

    private IEnumerator DisplayTextSlowly(int time)
    {
        promptMessage.gameObject.GetComponent<CanvasGroup>().LeanAlpha(0, time).setLoopPingPong(1);
        yield return new WaitForSeconds(time);
        promptMessage.text = ShowPhrase(phraseCollection[gameID]);
    }

    private IEnumerator DisplayStartingText()
    {
        promptMessage.gameObject.GetComponent<CanvasGroup>().LeanAlpha(0, 2f).setDelay(1).setLoopPingPong(1);
        yield return new WaitForSeconds(3.2f);
        promptMessage.text = ShowPhrase(phraseCollection[gameID]);
    }
}