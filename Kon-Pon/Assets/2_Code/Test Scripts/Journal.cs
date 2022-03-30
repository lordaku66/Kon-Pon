using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TODO: Add browseing buttons on journal page

public enum JournalPage
{
    Menu,
    Words,
    Settings,
    AreYouSure
}
public class Journal : MonoBehaviour
{
    private event Action OnJournalSelected;
    private event Action OnSettingsSelected;
    private event Action OnSaveSelected;
    private event Action OnQuitSelected;

    public event Action PageNext;
    public event Action PagePrevious;

    public JournalPage currentPage = JournalPage.Menu;
    private UIManager uiManager;
    private CanvasGroup cg;

    // UI Button & Alpha 
    [Header("Journal UI Button")]
    [SerializeField] private Button journalButton;
    private CanvasGroup buttonColor;

    // Book Image & Alpha
    [Header("Journal Object")]
    [SerializeField] private GameObject journal;
    private CanvasGroup journalColor;

    // Pause Menu Page & Alpha
    [Header("Menu Page - Settings, Save, Quit Etc")]
    [SerializeField] private GameObject menuPage;
    private CanvasGroup menuColor;

    // Learned Word Page & Alpha
    [Header("Collected Words Page & Title")]
    [SerializeField] private GameObject wordPage;
    private CanvasGroup wordColor;

    // 'Learned Words' Title
    [SerializeField] private TMP_Text learnedWordsTitle;

    [Header("Page Arrows")]
    [SerializeField] private GameObject leftArrow;
    private Button pageLeft;
    [SerializeField] private GameObject rightArrow;
    private Button pageRight;


    // Dark Overlay
    [Header("Journal BG")]
    [SerializeField] private CanvasGroup bgColor;

    [SerializeField] private float fadeTime = 0.5f;

    [SerializeField] private GameObject underConstruction;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    private void OnEnable()
    {
        // Subscribe to UI Manager events
        uiManager.OnJournalOpen += ShowJournal;
        uiManager.OnJournalClosed += ResumeGame;
        uiManager.OnJournalGoBack += ReturnToPreviousMenu;

        // Subscribe
        OnJournalSelected += HideMenu;
        OnSettingsSelected += HideMenu;
        OnSaveSelected += HideMenu;

        // Get page alphas
        buttonColor = journalButton.gameObject.GetComponent<CanvasGroup>();
        journalColor = journal.GetComponent<CanvasGroup>();
        wordColor = wordPage.GetComponent<CanvasGroup>();
        menuColor = menuPage.GetComponent<CanvasGroup>();

        // Hide the jounral
        journalColor.alpha = 0;

        // Hide Page Buttons
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    private void OnDisable()
    {
        uiManager.OnJournalOpen -= ShowJournal;
        uiManager.OnJournalClosed -= ResumeGame;
        uiManager.OnJournalGoBack -= ReturnToPreviousMenu;
    }

    public void ShowJournal()
    {
        buttonColor.LeanAlpha(0, fadeTime).setOnComplete(() => journalButton.gameObject.SetActive(false));
        journalColor.LeanAlpha(1, fadeTime);
        bgColor.LeanAlpha(1, fadeTime);
    }

    public void ResumeGame()
    {
        journalButton.gameObject.SetActive(true);
        buttonColor.LeanAlpha(1, fadeTime);
        journalColor.LeanAlpha(0, fadeTime);
        bgColor.LeanAlpha(0, fadeTime);
        uiManager.notbookClosing = true;
    }

    public void OnJournal()
    {
        OnJournalSelected?.Invoke();

        currentPage = JournalPage.Words;
        wordPage.SetActive(true);
        wordColor.LeanAlpha(1, fadeTime);

        leftArrow.SetActive(true);
        rightArrow.SetActive(true);

        learnedWordsTitle.gameObject.SetActive(true);
    }

    private void HideJournal()
    {
        wordPage.SetActive(false);
        wordColor.alpha = 0;

        leftArrow.SetActive(true);
        rightArrow.SetActive(true);

        learnedWordsTitle.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        currentPage = JournalPage.Menu;
        menuPage.SetActive(true);
        menuColor.LeanAlpha(1, fadeTime);
    }
    public void HideMenu()
    {
        menuPage.SetActive(false);
        menuColor.alpha = 0;
    }

    // Called on 'ESC' pressed
    public void ReturnToPreviousMenu()
    {
        if (currentPage == JournalPage.Menu)
        {
            ResumeGame();
        }
        else if (currentPage == JournalPage.Words)
        {
            HideJournal();
        }
        else if (currentPage == JournalPage.Settings)
        {
            //...
        }
        else
        {
            //...
        }

        ShowMenu();
    }

    public void RightButton()
    {
        PageNext?.Invoke();
    }

    public void LeftButton()
    {
        PagePrevious?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnderConstrucion()
    {

    }
}
