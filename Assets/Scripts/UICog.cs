using UnityEngine;
using UnityEngine.EventSystems;

namespace Crenix
{
    public class UICog : MonoBehaviour, IGrabbable, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Color color;

        new private Camera camera;
        private Vector3 mouseOffset;

        public Color Color => color;
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector2 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public IGrabber Grabber { get; private set; }

        void Awake()
        {
            // Camera.main should have an internal cache in Unity 2020.1+, not sure about Unity 2019.4
            camera = Camera.main;
        }

        public void SetActive(bool value) 
            => gameObject.SetActive(value);

        public void SetParent(Transform parent)
            => transform.SetParent(parent);

        public void OnPointerDown(PointerEventData eventData)
        {
            mouseOffset = camera.ScreenToWorldPoint(eventData.position) - transform.position;
            mouseOffset.z = 0;

            // Make the cog the top most item
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos = camera.ScreenToWorldPoint(eventData.position) - mouseOffset;
            pos.z = 0;
            transform.position = pos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InteractionManager.Drop(this);
        }

        public void SetOwner(IGrabber grabber)
        {
            this.Grabber = grabber;
        }
    }
}
