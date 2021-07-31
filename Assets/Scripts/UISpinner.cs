using System.Collections;
using UnityEngine;

namespace Crenix
{
    public class UISpinner : MonoBehaviour
    {
        [SerializeField] private Vector3 spinDirection;

        private void OnEnable()
        {
            MiniGameEvents.OnMiniGameFinished += StartSpinning;
            MiniGameEvents.OnMiniGameRegressed += StopSpinning;
            MiniGameEvents.OnMiniGameReset += StopSpinning;
        }

        private void OnDisable()
        {
            MiniGameEvents.OnMiniGameFinished -= StartSpinning;
            MiniGameEvents.OnMiniGameRegressed -= StopSpinning;
            MiniGameEvents.OnMiniGameReset -= StopSpinning;
        }

        public void StartSpinning()
        {
            StartCoroutine(SpinningRoutine());
        }

        public void StopSpinning()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpinningRoutine()
        {
            while(true)
            {
                transform.Rotate(spinDirection * Time.deltaTime);
                yield return null;
            }
        }
    }
}
