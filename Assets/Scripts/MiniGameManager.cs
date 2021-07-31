using UnityEngine;
using System.Linq;

namespace Crenix
{
    public class MiniGameManager : MonoBehaviour
    {
        [SerializeField] private UISlot[] worldSlots;

        void OnEnable()
        {
            foreach(var slot in worldSlots)
            {
                slot.OnActivate += OnCogGrabbed;
                slot.OnDeactivate += OnCogReleased;
            }
        }

        void OnDisable()
        {
            foreach (var slot in worldSlots)
            {
                slot.OnActivate -= OnCogGrabbed;
                slot.OnDeactivate -= OnCogReleased;
            }
        }

        private void OnCogGrabbed(UISlot slot)
        {
            if (worldSlots.Where(x => !x.Active).Count() > 0)
                return;
            MiniGameEvents.OnMiniGameFinished.Invoke();
        }

        private void OnCogReleased(UISlot slot)
        {
            MiniGameEvents.OnMiniGameRegressed.Invoke();
        }
    }
}
