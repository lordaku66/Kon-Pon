using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Konpon 2021
///     
/// Controls the color of the phrase prompt box during a game of Karuta. 
/// The pormpt box responds to player input by changing its colors to suit gamestate.
/// 
/// The box makes use of the OnCardValid, OnCardInvalid and OnPhraseShown events from 'KarutaGame.cs'
/// to change its colors.  
/// 
/// Author: Jacques Visser
/// </summary>
public class PhraseBoxBehaivior : MonoBehaviour
{
    private Image uiBox;
    [SerializeField] private Color normalBoxColor = new Color(0, 0.9960785f, 0.9882354f, 1);
    [SerializeField] private Color correctCardColor = new Color(0.3345942f, 0.9716981f, 0.4533708f, 1);
    [SerializeField] private Color incorrectCardColor = new Color(0.8962264f, 0.2555484f, 0.2156016f, 1);
    private bool isCorrect;
    private KarutaGame gameInstance;
    public Button phraseButton;
    public bool isKana;
    public bool isRomaji;

    private void Awake()
    {
        gameInstance = GetComponentInParent<KarutaGame>();
        uiBox = GetComponent<Image>();
        phraseButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        gameInstance.OnCardValid += ShowGreen;
        gameInstance.OnCardInvalid += ShowRed;
        gameInstance.OnPhraseShown += ShowNormal;
    }

    private void OnDisable()
    {
        gameInstance.OnCardValid -= ShowGreen;
        gameInstance.OnCardInvalid -= ShowRed;
    }

    public void ShowGreen()
    {
        uiBox.color = Color.Lerp(uiBox.color, correctCardColor, 1 * Time.time);
    }

    public void ShowRed()
    {
        uiBox.color = Color.Lerp(uiBox.color, incorrectCardColor, 1 * Time.time);
    }

    public void ShowNormal()
    {
        uiBox.color = Color.Lerp(uiBox.color, normalBoxColor, 1 * Time.time);
    }

    public void ChangePhrase()
    {
        if (isKana)
        {
            isRomaji = true;
            isKana = false;
        }
    }
}
