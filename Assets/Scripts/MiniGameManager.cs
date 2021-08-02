using UnityEngine;
using System.Linq;

namespace Crenix
{
    public class MiniGameManager : MonoBehaviour
    {
        [SerializeField] private WorldSlot[] worldSlots;

        void OnEnable()
        {
            foreach (var slot in worldSlots)
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

        private void OnCogGrabbed(WorldSlot slot)
        {
            bool allActive = worldSlots.All(x => x.IsActive);
            if (!allActive)
                return;
            MiniGameEvents.OnMiniGameFinished.Invoke();
        }

        private void OnCogReleased(WorldSlot slot)
        {
            MiniGameEvents.OnMiniGameRegressed.Invoke();
        }
    }
}
