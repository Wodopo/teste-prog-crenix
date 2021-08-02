using UnityEngine;

namespace Crenix
{
    public class UISlot : MonoBehaviour, IGrabber
    {
        [SerializeField] private Vector2 offset;

        public bool IsOccupied => Grabbable != null;
        public bool IsActive { get; private set; } = false;
        public IGrabbable Grabbable { get; private set; }

        private IGrabbable originalGrabbable;

        void Awake()
        {
            var grabbable = GetComponentInChildren<IGrabbable>();
            if (grabbable == null)
                return;
            originalGrabbable = grabbable;
            InteractionManager.Grab(grabbable, this);
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
            IsActive = false;
            Release();
            originalGrabbable.SetActive(true);
            InteractionManager.Grab(originalGrabbable, this);
        }

        public void Grab(IGrabbable grababble)
        {
            this.Grabbable = grababble;
            grababble.SetParent(this.transform);
            grababble.LocalPosition = Vector2.zero + offset;
            IsActive = true;
        }

        public void Release()
        {
            this.Grabbable = null;
            IsActive = false;
        }
    }
}
