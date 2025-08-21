using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RubikStudio.Yahtzee
{
    public class EnumGame : MonoBehaviour
    {

    }
    public enum ScoreCategory
    {
        Ones, Twos, Threes, Fours, Fives, Sixes,
        ThreeOfAKind, FourOfAKind, FullHouse,
        SmallStraight, LargeStraight, Yahtzee, Chance
    }
}

