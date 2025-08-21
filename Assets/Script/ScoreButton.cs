using UnityEngine;
using UnityEngine.UI;
namespace RubikStudio.Yahtzee
{
    public class ScoreButton : MonoBehaviour
    {
        public ScoreCategory Category;
        public Text label;
        public Button button;

        public bool Used { get; private set; }
        public int CurrentScore { get; private set; }

        GameplayController game;

        void Awake()
        {
            game = FindObjectOfType<GameplayController>();
            if (button) button.onClick.AddListener(OnClick);
            Refresh();
        }

        public void LockWithScore(int score)
        {
            Used = true;
            CurrentScore = score;
            Refresh();
            if (button) button.interactable = false;
        }

        public void RefreshPreview(int score)
        {
            if (Used) return;
            if (label) label.text = $"{Category} ({score})";
        }

        public void ResetSlot()
        {
            Used = false;
            CurrentScore = 0;
            if (button) button.interactable = true;
            Refresh();
        }

        void Refresh()
        {
            if (label)
            {
                label.text = Used ? $"{Category}: {CurrentScore}" : $"{Category}";
            }
        }

        void OnClick()
        {
            if (Used) return;
            if (game) game.SelectCategory(this);
        }
    }

}
