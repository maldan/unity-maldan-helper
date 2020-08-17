using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void UpHandler(PointerEventData eventData);
        public delegate void DownHandler(PointerEventData eventData);
        public delegate void HoverEnterHandler(PointerEventData eventData);
        public delegate void HoverExitHandler(PointerEventData eventData);
        
        public event UpHandler AddUpListener;
        public event DownHandler AddDownListener;
        public event HoverEnterHandler AddHoverEnterListener;
        public event HoverExitHandler AddHoverExitListener;

        public Action OnceDownListener;
        public Action OnceUpListener;
        public Action OnceOverListener;
        public Action OnceOutListener;
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!GetComponent<Button>().interactable) return;
            AddUpListener?.Invoke(eventData);
            
            OnceUpListener?.Invoke();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!GetComponent<Button>().interactable) return;
            AddDownListener?.Invoke(eventData);
            
            OnceDownListener?.Invoke();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!GetComponent<Button>().interactable) return;
            AddHoverEnterListener?.Invoke(eventData);
            
            OnceOverListener?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!GetComponent<Button>().interactable) return;
            AddHoverExitListener?.Invoke(eventData);
            
            OnceOutListener?.Invoke();
        }
    }
}