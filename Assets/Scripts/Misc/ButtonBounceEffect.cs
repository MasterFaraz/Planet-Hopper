using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonBounceEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    public float scaleDownFactor = 0.9f;
    public float bounceDuration = 0.2f;
    public AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        transform.localScale = originalScale * scaleDownFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(BounceBack());
    }

    private IEnumerator BounceBack()
    {
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < bounceDuration)
        {
            float t = elapsed / bounceDuration;
            float curveValue = bounceCurve.Evaluate(t);
            transform.localScale = Vector3.LerpUnclamped(startScale, originalScale, curveValue);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
