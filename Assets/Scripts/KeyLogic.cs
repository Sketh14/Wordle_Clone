using UnityEngine;

namespace Wordle_Clone
{
    public class KeyLogic : MonoBehaviour
    {
        [SerializeField] private byte keyIndex;

        [Header("Script Reference")]
        [SerializeField] private GameLogic localGameLogic;

        private void OnEnable()
        {
            localGameLogic.OnGameRestart += ResetKey;
        }

        private void OnDisable()
        {
            localGameLogic.OnGameRestart -= ResetKey;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        //On the key button, under Keys/MainCanvas
        public void KeyPressed()
        {
            if (GameManager.instance.gameWon)
                return;

            localGameLogic.OnKeyPressed?.Invoke(keyIndex);

            if (keyIndex == 254)                     //For BackSpace
            {
                if (GameManager.instance.currentLetterIndex != 0)
                    GameManager.instance.currentLetterIndex--;
            }
            else if (keyIndex == 255)                     //Ignore Enter
            {
                //Check if the row if fully filled
                if (GameManager.instance.currentLetterIndex == 5)
                {
                    //Moved to ChosenLetter
                }
            }
            else
            {
                if (GameManager.instance.currentLetterIndex < 5)
                    GameManager.instance.currentLetterIndex++;
                //Debug.Log($"Key with index {keyIndex} was pressed");
            }

        }

        //If anything needs to be reset
        private void ResetKey()
        {

        }
    }
}