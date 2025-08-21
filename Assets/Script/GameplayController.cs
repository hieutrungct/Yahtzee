using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace RubikStudio.Yahtzee
{
    public class GameplayController : MonoBehaviour
    {
        public DieUI[] dice;
        public ScoreButton[] scoreButtons;
        public Button rollButton;
        public Text rollsLeftText;
        public Text roundText;

        public Text upperTotalText;
        public Text lowerTotalText;
        public Text bonusText;
        public Text grandTotalText;

        const int MaxRollsPerTurn = 3;
        int rollsLeft = MaxRollsPerTurn;
        int roundIndex = 1;
        bool hasRolled;

        Dictionary<ScoreCategory, int> scores = new Dictionary<ScoreCategory, int>();

        void Start()
        {
            NewGame();
        }

        public void NewGame()
        {
            scores.Clear();
            foreach (var s in scoreButtons) s.ResetSlot();
            roundIndex = 1;
            StartTurn();
            UpdateTotalsUI();
        }

        void StartTurn()
        {
            hasRolled = false;
            rollsLeft = MaxRollsPerTurn;
            foreach (var d in dice) d.ResetDie();
            if (rollButton) rollButton.interactable = true;
            UpdateRollRoundUI();
        }

        void UpdateRollRoundUI()
        {
            if (rollsLeftText) rollsLeftText.text = $"Rolls Left: {rollsLeft}";
            if (roundText) roundText.text = $"Round: {roundIndex}/13";
        }

        public void OnRollClicked()
        {
            if (rollsLeft <= 0) return;
            foreach (var d in dice) d.Roll();
            hasRolled = true;
            rollsLeft--;
            if (rollButton) rollButton.interactable = rollsLeft > 0;
            UpdateRollRoundUI();
            PreviewScores();
        }

        void PreviewScores()
        {
            var vals = GetDiceValues();
            foreach (var s in scoreButtons)
            {
                if (!s.Used)
                {
                    int sc = ScoreCalculator.ScoreForCategory(vals, s.Category);
                    s.RefreshPreview(sc);
                }
            }
        }

        List<int> GetDiceValues() => dice.Select(d => d.Value == 0 ? Random.Range(1, 7) : d.Value).ToList();

        public void SelectCategory(ScoreButton slot)
        {
            if (!hasRolled) return;
            var vals = dice.Select(d => d.Value).ToList();
            int sc = ScoreCalculator.ScoreForCategory(vals, slot.Category);
            scores[slot.Category] = sc;
            slot.LockWithScore(sc);
            NextTurnOrEnd();
        }

        void NextTurnOrEnd()
        {
            if (roundIndex >= 13)
            {
                EndGame();
                return;
            }
            roundIndex++;
            StartTurn();
            UpdateTotalsUI();
        }

        void EndGame()
        {
            if (rollButton) rollButton.interactable = false;
            foreach (var d in dice) d.SetHeld(false);
            UpdateTotalsUI();
            foreach (var s in scoreButtons) if (!s.Used) s.button.interactable = false;
        }

        void UpdateTotalsUI()
        {
            int upper = scores.Where(kv => ScoreCalculator.IsUpper(kv.Key)).Sum(kv => kv.Value);
            int lower = scores.Where(kv => !ScoreCalculator.IsUpper(kv.Key)).Sum(kv => kv.Value);
            int bonus = upper >= 63 ? 35 : 0;
            int grand = upper + lower + bonus;

            if (upperTotalText) upperTotalText.text = $"Upper: {upper}";
            if (lowerTotalText) lowerTotalText.text = $"Lower: {lower}";
            if (bonusText) bonusText.text = $"Bonus: {bonus}";
            if (grandTotalText) grandTotalText.text = $"Total: {grand}";
        }
    }
}

