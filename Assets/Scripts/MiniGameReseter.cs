using UnityEngine;

namespace Crenix
{
    public class MiniGameReseter : MonoBehaviour
    {
        public void RaiseResetEvent()
        {
            MiniGameEvents.OnMiniGameReset?.Invoke();
        }
    } 
}
