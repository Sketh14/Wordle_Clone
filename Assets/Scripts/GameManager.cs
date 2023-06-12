using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wordle_Clone
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager instance { get { return _instance; } }

        public GameLogic gameLogicReference;
        public byte currentRowIndex, currentLetterIndex, correctLettersChosen;
        public bool gameWon;

        private void Awake()
        {
            if (instance == null && _instance != this)
                _instance = this;
            else
                Destroy(instance);
        }

        private void OnEnable()
        {
            currentLetterIndex = 0;
        }
    }
}