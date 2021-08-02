using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Crenix
{
    public class WorldSlot : MonoBehaviour, IGrabber, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private SpriteRenderer worldCog;

        public event Action<WorldSlot> OnActivate;
        public event Action<WorldSlot> OnDeactivate;

        private bool isActive = false;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive == value)
                    return;
                isActive = value;
                var action = isActive ? OnActivate : OnDeactivate;
                action?.Invoke(this);
            }
        }

        public bool IsOccupied => Grabbable != null;
        public IGrabbable Grabbable { get; private set; }

        void Awake()
        {
            worldCog.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            MiniGameEvents.OnMiniGameReset += ResetState;
        }

        void OnDisable()
        {
            MiniGameEvents.OnMiniGameReset -= ResetState;
        }

        private void ResetState()
        {
            worldCog.gameObject.SetActive(false);
            IsActive = false;
            Release();
        }

        public void Grab(IGrabbable grabbable)
        {
            this.Grabbable = grabbable;
            grabbable.SetActive(false);
            grabbable.Position = transform.position;
            worldCog.color = grabbable.Color;
            worldCog.gameObject.SetActive(true);
            IsActive = true;
        }

        public void Release()
        {
            this.Grabbable = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Grabbable == null)
                return;

            worldCog.gameObject.SetActive(false);
            Grabbable.SetActive(true);
            IsActive = false;

            // Pass along the event
            Grabbable.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Grabbable == null)
                return;

            // Pass along the event
            Grabbable.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Grabbable == null)
                return;

            // Pass along the event
            Grabbable.OnPointerUp(eventData);
        }
    }
}
