using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [HideInInspector] public Transform parentToReturnTo = null;
    [HideInInspector] public Transform placeHolderParent = null;

    [SerializeField] private ItemContainer itemContainer;

    private GameObject placeHolder = null;

    public void OnBeginDrag(PointerEventData eventData) {
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        this.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent) {
            placeHolder.transform.SetParent(placeHolderParent);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        int newSiblingIndex = placeHolderParent.childCount;


        this.transform.SetParent(parentToReturnTo);
        for (int i = 0; i < placeHolderParent.childCount; i++) {
            var rect = (placeHolderParent.GetChild(i) as RectTransform);
            var pos = rect.InverseTransformPoint(eventData.position);

            if (rect.rect.Contains(pos)) {
                newSiblingIndex = i;

                var placeHolderIndex = placeHolder.transform.GetSiblingIndex();
                placeHolderParent.GetChild(newSiblingIndex).SetSiblingIndex(placeHolderIndex);

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex) {
                    newSiblingIndex++;
                }

                this.transform.SetSiblingIndex(newSiblingIndex);

                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                Destroy(placeHolder);
                EventManager.HandleOnItemSwapped();
                return;
            }
        }

        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());

        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);
    }
}