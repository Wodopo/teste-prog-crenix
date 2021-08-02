using UnityEngine;
using UnityEngine.EventSystems;

namespace Crenix
{
    public interface IGrabbable
    {
        Vector2 Position { get; set; }
        Vector2 LocalPosition { get; set; }
        Color Color { get; }
        IGrabber Grabber { get; }

        void OnPointerDown(PointerEventData eventData);
        void OnDrag(PointerEventData eventData);
        void OnPointerUp(PointerEventData eventData);

        void SetActive(bool value);
        void SetOwner(IGrabber grabber);
        void SetParent(Transform parent);
    } 
}