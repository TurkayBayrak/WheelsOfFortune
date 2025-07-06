using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollSetter : MonoBehaviour
{
    public static void SetScroll(ScrollRect scrollRect, int childCount, float contentChildWidth, int childCountLimit)
    {
        if (childCount > childCountLimit)
        {
            scrollRect.horizontal = true;
            var spacingTotal = scrollRect.content.GetComponent<HorizontalLayoutGroup>().spacing * (childCount - 1);
            var deltaSize = contentChildWidth * childCount + spacingTotal + 80;
            var contentRectTransform = scrollRect.content.GetComponent<RectTransform>();
            contentRectTransform.DOSizeDelta(new Vector2(deltaSize, contentRectTransform.sizeDelta.y), 0);
            contentRectTransform.DOAnchorPosX(-(scrollRect.GetComponent<RectTransform>().sizeDelta.x * .5f), 0);
        }
        else
        {
            scrollRect.horizontal = false;
            var spacingTotal = scrollRect.content.GetComponent<HorizontalLayoutGroup>().spacing * (childCount - 1);
            var deltaSize = contentChildWidth * childCount + spacingTotal + 80;
            var contentRectTransform = scrollRect.content.GetComponent<RectTransform>();
            contentRectTransform.DOSizeDelta(new Vector2(deltaSize, contentRectTransform.sizeDelta.y), 0);
            contentRectTransform.DOAnchorPosX(-deltaSize * .5f, 0);
        }
    }
}
