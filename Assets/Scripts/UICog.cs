using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crenix
{
    public class UICog : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
    {
        public static UICog LastDraggedCog { get; private set; }

        [SerializeField] private Sprite uiSprite;
        [SerializeField] private Sprite worldSprite;

        new private Camera camera;
        private Image img;
        private Vector3 mouseOffset;
        private bool dragging = false;
        private UISlot originalSlot = null;

        private UISlot slot;
        public UISlot Slot
        {
            get => slot;
            set
            {
                slot = value;
                if (originalSlot == null)
                    originalSlot = value;
            }
        }

        void Awake()
        {
            img = GetComponent<Image>();

            // Camera.main should have an internal cache in Unity 2020.1+, not sure about Unity 2019.4
            camera = Camera.main;
        }

        void OnEnable()
        {
            MiniGameEvents.OnMiniGameReset += OnMiniGameReset;
        }

        void OnDisable()
        {
            MiniGameEvents.OnMiniGameReset -= OnMiniGameReset;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mouseOffset = camera.ScreenToWorldPoint(eventData.position) - transform.position;
            mouseOffset.z = 0;

            // Prevent the cog from grabbing the IDropHandler event
            img.raycastTarget = false;

            // Make the cog the top most item
            transform.SetParent(transform.root);

            transform.localRotation = Quaternion.identity;

            // Revert to the UI Icon
            SetUISprite();

            Slot.Deactivate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            LastDraggedCog = this;
            dragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos = camera.ScreenToWorldPoint(eventData.position) - mouseOffset;
            pos.z = 0;
            transform.position = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // The event from IDropHandler already happened on the slot and changed (or not) the cog's slot reference
            Slot.GrabCog(this);
            dragging = false;
        }

        // OnPointerUP happens before OnEndDrag and OnDrop (UISlot)
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!dragging)
                Slot.GrabCog(this);

            // Makes the cog draggable again
            img.raycastTarget = true;
        }

        public void SetUISprite()
        {
            img.sprite = uiSprite;
        }

        public void SetWorldSprite()
        {
            img.sprite = worldSprite;
        }

        private void OnMiniGameReset()
        {
            slot.Deactivate();
            transform.localRotation = Quaternion.identity;
            slot.ReleaseCog();
            slot = originalSlot;
            slot.GrabCog(this);
            SetUISprite();
        }
    }
}
