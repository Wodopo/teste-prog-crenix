using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Crenix
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool useWorldSprite;
        [SerializeField] private Vector3 offset;

        public UICog Cog { get; private set; }

        private bool active;
        public bool Active
        { 
            get => active;
            set
            {
                if (active == value)
                    return;
                active = value;
                var evnt = active ? OnActivate : OnDeactivate;
                evnt?.Invoke(this);
            }
        }

        public event Action<UISlot> OnActivate;
        public event Action<UISlot> OnDeactivate;
        public event Action<UISlot, UICog> OnCogGrabbed;
        public event Action<UISlot> OnCogReleased;

        void Awake()
        {
            // Attempts to grab a Cog when the scene is loaded
            var cog = GetComponentInChildren<UICog>();
            if (cog == null)
                return;
            cog.Slot = this;
            GrabCog(cog);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Cog != null)
                return;

            UICog.LastDraggedCog.Slot.ReleaseCog();
            UICog.LastDraggedCog.Slot = this;
        }

        public void GrabCog(UICog cog)
        {
            Cog = cog;
            cog.transform.SetParent(this.transform);
            cog.transform.localPosition = offset;
            cog.transform.localRotation = Quaternion.identity;
            
            if (useWorldSprite)
                cog.SetWorldSprite();

            Active = true;
            OnCogGrabbed?.Invoke(this, cog);
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void ReleaseCog()
        {
            Cog = null;
            OnCogReleased?.Invoke(this);
        }
    }
}
