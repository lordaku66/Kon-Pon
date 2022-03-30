using UnityEngine;

public class Highlight2D : MonoBehaviour
{
    public Sprite outlineImage;
    //public RuntimeAnimatorController highlightAnimation;
    //private Animator animator;
    private Transform objectHighlight;
    private SpriteRenderer highlightRenderer;
    private Material material;
    [SerializeField] private Color highlightColor = new Color32(255, 0, 0, 255);

    private LanguageLabels labelSystem;

    private void Awake()
    {
        labelSystem = GetComponent<LanguageLabels>();
    }


    private void OnEnable()
    {
        objectHighlight = new GameObject().transform;
        objectHighlight.parent = gameObject.transform;
        objectHighlight.gameObject.name = (gameObject.name + "'s highlight");
        objectHighlight.rotation = transform.parent.rotation;
        objectHighlight.localPosition = Vector3.zero;

        highlightRenderer = objectHighlight.gameObject.AddComponent<SpriteRenderer>();
        highlightRenderer.sprite = outlineImage;
        highlightRenderer.color = highlightColor;
        highlightRenderer.enabled = false;

        //gameObject.GetComponent<Material>().

        labelSystem.OnHover += ShowHighlight;
        labelSystem.OnHoverExit += HideHighlight;
    }

    public void ShowHighlight()
    {
        highlightRenderer.enabled = true;

        // animator = objectHighlight.gameObject.AddComponent<Animator>();
        // animator.runtimeAnimatorController = highlightAnimation;
    }

    private void HideHighlight()
    {
        highlightRenderer.enabled = false;
    }

    private void Update()
    {
        highlightRenderer.color = highlightColor;
    }
}
