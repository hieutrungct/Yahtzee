using System.Collections.Generic;
using System.Linq;
namespace RubikStudio.Yahtzee
{
    public static class ScoreCalculator
    {
        static int Sum(IEnumerable<int> xs) => xs.Sum();
        static Dictionary<int, int> Counts(IEnumerable<int> xs)
        {
            var d = new Dictionary<int, int>();
            foreach (var v in xs)
            {
                if (!d.ContainsKey(v)) d[v] = 0;
                d[v]++;
            }
            return d;
        }

        static bool HasKind(Dictionary<int, int> c, int n) => c.Values.Any(v => v >= n);

        static bool IsFullHouse(Dictionary<int, int> c)
        {
            var vs = c.Values.OrderBy(x => x).ToArray();
            return vs.Length == 2 && vs[0] == 2 && vs[1] == 3;
        }

        static bool IsSmallStraight(HashSet<int> s)
        {
            return (s.Contains(1) && s.Contains(2) && s.Contains(3) && s.Contains(4)) ||
                   (s.Contains(2) && s.Contains(3) && s.Contains(4) && s.Contains(5)) ||
                   (s.Contains(3) && s.Contains(4) && s.Contains(5) && s.Contains(6));
        }

        static bool IsLargeStraight(HashSet<int> s)
        {
            return (s.SetEquals(new HashSet<int> { 1, 2, 3, 4, 5 }) ||
                    s.SetEquals(new HashSet<int> { 2, 3, 4, 5, 6 }));
        }

        public static int ScoreForCategory(List<int> dice, ScoreCategory cat)
        {
            var cnt = Counts(dice);
            var set = new HashSet<int>(dice);
            switch (cat)
            {
                case ScoreCategory.Ones: return dice.Where(x => x == 1).Sum();
                case ScoreCategory.Twos: return dice.Where(x => x == 2).Sum();
                case ScoreCategory.Threes: return dice.Where(x => x == 3).Sum();
                case ScoreCategory.Fours: return dice.Where(x => x == 4).Sum();
                case ScoreCategory.Fives: return dice.Where(x => x == 5).Sum();
                case ScoreCategory.Sixes: return dice.Where(x => x == 6).Sum();
                case ScoreCategory.ThreeOfAKind: return HasKind(cnt, 3) ? Sum(dice) : 0;
                case ScoreCategory.FourOfAKind: return HasKind(cnt, 4) ? Sum(dice) : 0;
                case ScoreCategory.FullHouse: return IsFullHouse(cnt) ? 25 : 0;
                case ScoreCategory.SmallStraight: return IsSmallStraight(set) ? 30 : 0;
                case ScoreCategory.LargeStraight: return IsLargeStraight(set) ? 40 : 0;
                case ScoreCategory.Yahtzee: return cnt.Values.Any(v => v == 5) ? 50 : 0;
                case ScoreCategory.Chance: return Sum(dice);
            }
            return 0;
        }

        public static bool IsUpper(ScoreCategory c)
        {
            return c == ScoreCategory.Ones || c == ScoreCategory.Twos || c == ScoreCategory.Threes ||
                   c == ScoreCategory.Fours || c == ScoreCategory.Fives || c == ScoreCategory.Sixes;
        }
    }

}
