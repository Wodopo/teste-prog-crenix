using TMPro;
using UnityEngine;

namespace Crenix
{
    public class Nugget : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField, TextArea] private string instructions;
        [SerializeField, TextArea] private string congratulations;
        private readonly int newTextHash = Animator.StringToHash("new-text");
        private bool finished;

        void Start()
        {
            Instruct();
        }

        void OnEnable()
        {
            MiniGameEvents.OnMiniGameRegressed += OnMiniGameRegressed;
            MiniGameEvents.OnMiniGameReset += OnMiniGameRegressed;
            MiniGameEvents.OnMiniGameFinished += OnMiniGameFinished;
        }

        void OnDisable()
        {
            MiniGameEvents.OnMiniGameRegressed -= OnMiniGameRegressed;
            MiniGameEvents.OnMiniGameReset -= OnMiniGameRegressed;
            MiniGameEvents.OnMiniGameFinished -= OnMiniGameFinished;
        }

        private void OnMiniGameRegressed()
        {
            if (!finished)
                return;

            Instruct();
            finished = false;
        }

        private void OnMiniGameFinished()
        {
            Congratulate();
            finished = true;
        }

        private void Instruct()
        {
            textField.text = instructions;
            animator.SetTrigger(newTextHash);
        }

        private void Congratulate()
        {
            textField.text = congratulations;
            animator.SetTrigger(newTextHash);
        }
    } 
}
