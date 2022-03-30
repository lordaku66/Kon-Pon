using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalData : MonoBehaviour
{
    private Journal jounral;

    [SerializeField] List<LanguageLabels[]> pages = new List<LanguageLabels[]>();
    [SerializeField] LanguageLabels[] pageOne;
    [SerializeField] LanguageLabels[] pageTwo;
    [SerializeField] private JournalEntry[] prefabSlots = new JournalEntry[4];
    [SerializeField] private int pageIndex = 0;

    // public Dictionary<string, bool> journalEntries = new Dictionary<string, bool>();
    [SerializeField] private int wordsLearned;
    private string unknownString = "???";

    private void Awake()
    {
        jounral = GetComponent<Journal>();
    }

    private void OnEnable()
    {
        pages.Add(pageOne);
        pages.Add(pageTwo);

        SetAllJournalData();

        jounral.PageNext += ShowNextWords;
        jounral.PagePrevious += ShowPrevWords;
    }

    public void SetAllJournalData()
    {
        for (int j = 0; j < prefabSlots.Length; j++)
        {
            if (pages[pageIndex][j] != null)
            {
                prefabSlots[j].title.text = pages[pageIndex][j].romajiWord;
                prefabSlots[j].eng.text = pages[pageIndex][j].englishWord;
                prefabSlots[j].kan.text = pages[pageIndex][j].kanaWord;
                prefabSlots[j].img.sprite = pages[pageIndex][j].gameObject.GetComponent<SpriteRenderer>()
                .sprite;

                prefabSlots[j].img.preserveAspect = true;
            }
            else
            {
                prefabSlots[j].gameObject.GetComponent<CanvasGroup>().alpha = 0;
            }

        }
    }

    public void ShowNextWords()
    {
        if (pageIndex < pages.Count)
        {
            Debug.Log("Next Page");
            pageIndex++;
            SetAllJournalData();
        }
    }

    public void ShowPrevWords()
    {
        if (pageIndex > 0)
        {
            Debug.Log("Previous Page");
            pageIndex--;
            SetAllJournalData();
        }
    }

}
