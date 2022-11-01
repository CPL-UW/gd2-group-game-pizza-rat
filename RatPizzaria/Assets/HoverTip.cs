using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string tipToShow;
    private float timeToWait = 0.5f;

    public void OnPointerEnter(PointerEventData eventData) {
        StopCoroutine(StartTimer());
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopCoroutine(StartTimer());
        HoverTipManager.OnMouseLoseFocus();
    }

    private void ShowTip() {
        HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSeconds(timeToWait);
        ShowTip();
    }
}
