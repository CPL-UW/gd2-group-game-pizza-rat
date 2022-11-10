using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string tipToShow;
    private float timeToWait = 0.5f;
    private Boolean onHover = false;

    public void OnPointerEnter(PointerEventData eventData) {
        onHover = true;
        StopCoroutine(StartTimer());
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData) {
        onHover = false;
        StopCoroutine(StartTimer());
        HoverTipManager.OnMouseLoseFocus();
    }

    private void ShowTip() {
        if (tipToShow == null) return;
        if (onHover) HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSeconds(timeToWait);
        ShowTip();
    }
}
