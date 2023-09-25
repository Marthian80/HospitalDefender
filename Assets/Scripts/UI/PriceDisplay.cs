using UnityEngine;

public class PriceDisplay : MonoBehaviour
{
    [SerializeField] private bool doNotPlayOnStart = false;
    [SerializeField] private bool isReward = false;

    private Animator myAnimator;
    private Flash flash;
    private SpriteRenderer spriteRenderer;

    readonly int ACTIVATE_HASH = Animator.StringToHash("Activate");

    private void Awake()
    {
        flash = FindObjectOfType<Flash>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!doNotPlayOnStart)
        {
            FloatPrice();
        }
        if (isReward)
        {
            Destroy(gameObject, 3);
        }
    }

    public void FloatPrice()
    {
        spriteRenderer.enabled = true;
        myAnimator.transform.position = gameObject.transform.position;
        myAnimator.SetTrigger(ACTIVATE_HASH);
        StartCoroutine(flash.SlowFadeOutRoutine(spriteRenderer));                
    }
}