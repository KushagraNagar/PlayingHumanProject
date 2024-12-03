using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Author > Kush  
/// Date > 03.12.24
/// UIManager manages the UI, the button and popup open and close with animations 
/// </summary>
public class UIManager : MonoBehaviour
{
    public enum AnimationType
    {
        ScaleBounce,
        FadeIn,
        SlideFromTop,
        RotateIn
    }

    [System.Serializable]
    public class PopupConfig
    {
        public Button triggerButton; // Button to open the popup
        public GameObject popupWindow; // Popup window GameObject
        public Button closeButton; // Close button for the popup
        public Button backgroundCloseButton; // Close button for the popup
        public AnimationType animationType; // Animation type for this popup
        public float animationDuration = 0.5f; // Duration of the animation
    }

    [Header("Popups Configuration")]
    public PopupConfig[] popups; // Array of popups with their configurations

    private void Start()
    {
        foreach (var popup in popups)
        {
            if (popup.triggerButton != null && popup.popupWindow != null)
            {
                popup.popupWindow.SetActive(false); // Ensure the popup starts inactive
                popup.triggerButton.onClick.AddListener(() => ShowPopup(popup)); // Assign open event

                if (popup.closeButton != null)
                {
                    popup.closeButton.onClick.AddListener(() => ClosePopup(popup)); // Assign close event
                    popup.backgroundCloseButton.onClick.AddListener(() => ClosePopup(popup)); // Assign close event
                }
            }
        }
    }

    private void ShowPopup(PopupConfig config)
    {
        var popup = config.popupWindow;

        // Reset popup state
        popup.SetActive(true);
        popup.transform.localScale = Vector3.one;
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>() ?? popup.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        // Execute the selected opening animation
        switch (config.animationType)
        {
            case AnimationType.ScaleBounce:
                popup.transform.localScale = Vector3.zero;
                popup.transform.DOScale(Vector3.one, config.animationDuration).SetEase(Ease.OutBounce);
                break;

            case AnimationType.FadeIn:
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, config.animationDuration);
                break;

            case AnimationType.SlideFromTop:
                Vector3 startPos = popup.transform.position;
                popup.transform.position = new Vector3(startPos.x, Screen.height, startPos.z);
                popup.transform.DOMoveY(startPos.y, config.animationDuration).SetEase(Ease.OutCubic);
                break;

            case AnimationType.RotateIn:
                popup.transform.localScale = Vector3.zero;
                popup.transform.DOScale(Vector3.one, config.animationDuration).SetEase(Ease.OutBack);
                popup.transform.DORotate(Vector3.zero, config.animationDuration).From(new Vector3(0, 0, 180));
                break;
        }
    }

    private void ClosePopup(PopupConfig config)
    {
        var popup = config.popupWindow;

        // Execute the selected closing animation
        switch (config.animationType)
        {
            case AnimationType.ScaleBounce:
                popup.transform.DOScale(Vector3.zero, config.animationDuration).SetEase(Ease.InBack).OnComplete(() => popup.SetActive(false));
                break;

            case AnimationType.FadeIn:
                CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>() ?? popup.AddComponent<CanvasGroup>();
                canvasGroup.DOFade(0, config.animationDuration).OnComplete(() => popup.SetActive(false));
                break;

            case AnimationType.SlideFromTop:
                Vector3 startPos = popup.transform.position;
                popup.transform.DOMoveY(Screen.height, config.animationDuration).SetEase(Ease.InCubic).OnComplete(() => popup.SetActive(false));
                break;

            case AnimationType.RotateIn:
                popup.transform.DOScale(Vector3.zero, config.animationDuration).SetEase(Ease.InBack).OnComplete(() => popup.SetActive(false));
                popup.transform.DORotate(new Vector3(0, 0, 180), config.animationDuration);
                break;
        }
    }
}
