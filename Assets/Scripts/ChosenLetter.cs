using UnityEngine;
using UnityEngine.UI;

namespace Wordle_Clone
{
    public class ChosenLetter : MonoBehaviour
    {
        private char letterChosen;
        public char _letterIndex { get { return letterChosen; } }
        [SerializeField] private byte letterIndex, rowIndex;

        [SerializeField] private Animator letter_AC;
        public bool repeat, correctPlace, firstInstance;
        [SerializeField] private GameObject parentRow;

        [Header("UI")]
        [SerializeField] private TMPro.TMP_Text letter_Txt;
        [SerializeField] private Sprite[] bgSprites;
        [SerializeField] private UnityEngine.UI.Image letterBG;
        private Color ogColor;

        [Header("Script Reference")]
        [SerializeField] private GameLogic localGameLogic;

        private void OnEnable()
        {
            localGameLogic.OnKeyPressed += UpdateLetter;
            localGameLogic.OnGameRestart += ResetLetter;
            localGameLogic.OnLetterRepeat += CheckRepeat;
        }

        private void OnDisable()
        {
            localGameLogic.OnKeyPressed -= UpdateLetter;
            localGameLogic.OnGameRestart -= ResetLetter;
            localGameLogic.OnLetterRepeat -= CheckRepeat;
        }

        // Start is called before the first frame update
        void Start()
        {
            ogColor = letterBG.color;
            letterChosen = '0';
            letter_Txt.text = "";
            letterBG.sprite = bgSprites[0];
        }

        //decode using ASCII
        private void UpdateLetter(byte keyIndex, bool repetition)
        {
            if (keyIndex == 255)        //For Enter
            {
                if (GameManager.instance.currentLetterIndex == 5)
                {
                    if (rowIndex != GameManager.instance.currentRowIndex ||
                        letterIndex != localGameLogic.animateLetterIndex)           //Only the ones in the selected row
                    {
                        //Debug.Log($"REjected.\n rowIndex : {rowIndex}, currentRowIndex : {GameManager.instance.currentRowIndex}\n" +
                        //    $"letterIndex : {letterIndex}, animateLetterIndex : {localGameLogic.animateLetterIndex}");

                        return;
                    }

                    //Debug.Log($"Accepted.\n rowIndex : {rowIndex}, currentRowIndex : {GameManager.instance.currentRowIndex}\n" +
                    //    $"letterIndex : {letterIndex}, animateLetterIndex : {localGameLogic.animateLetterIndex}");

                    letter_AC.SetBool("Flip", true);
                    letter_AC.Play("Flip", 0);
                    Invoke(nameof(UpdateBG), 0.33f);
                    Invoke(nameof(UpdateAnimateLetterIndex), 0.6f);
                    //UpdateBG();
                }
            }
            else if (keyIndex == 254)       //For BackSpace
            {
                if (letterIndex == (GameManager.instance.currentLetterIndex - 1)       //As the currentLetterIndex is updated later
                    && rowIndex == GameManager.instance.currentRowIndex)
                {
                    letterChosen = '0';
                    letter_Txt.text = "";

                    string temp = "";
                    for (int i = 0; i < (localGameLogic.enteredWord.Length - 1); i++)
                    {
                        //Debug.Log($"Entered Letter : {localGameLogic.enteredWord[i]}");
                        temp += localGameLogic.enteredWord[i];
                    }

                    localGameLogic.enteredWord = temp;
                    //Debug.Log($"Pressed BackSpace. Erasing. New word : {localGameLogic.enteredWord}");
                }
            }
            else
            {
                if (letterIndex == GameManager.instance.currentLetterIndex
                    && rowIndex == GameManager.instance.currentRowIndex)
                {
                    letterChosen = (char)keyIndex;
                    letter_Txt.text = letterChosen.ToString();

                    localGameLogic.enteredWord += letterChosen;
                    //localGameLogic.enteredWord2[GameManager.instance.currentLetterIndex] = keyIndex;

                    if (!localGameLogic.checkLetterRepeat2.ContainsKey(keyIndex))
                        localGameLogic.checkLetterRepeat2.Add(keyIndex, 0);
                    //Debug.Log($"enteredWord : {localGameLogic.enteredWord}");
                }
            }
        }

        private void CheckRepeat(byte letterIndex, bool firstInstance, bool correctPlace)
        {
            if (this.letterIndex == letterIndex && rowIndex == GameManager.instance.currentRowIndex)
            {
                repeat = true;
                this.firstInstance = firstInstance;
                this.correctPlace = correctPlace;

                //Debug.Log($"letterIndex : {letterIndex}, repeat : {repeat}, firstInstance: {firstInstance}, correctPlace : {correctPlace}");
            }
        }

        private void UpdateBG()
        {
            //if (rowIndex == GameManager.instance.currentRowIndex)           //Only the ones in the selected row
                letterBG.sprite = bgSprites[1];

            if (correctPlace)
            {
                letterBG.color = Color.green;
                GameManager.instance.correctLettersChosen++;
                if (GameManager.instance.correctLettersChosen == 5)
                {
                    GameManager.instance.gameWon = true;
                    localGameLogic.OnGameWon?.Invoke();
                }
                return;
            }
            else if (firstInstance)
            {
                for (int i = 0; i < parentRow.transform.childCount; i++)
                {
                    if (i != letterIndex)
                    {
                        char tempLetter = parentRow.transform.GetChild(i).GetComponent<ChosenLetter>()._letterIndex;
                        if (letterChosen.Equals(tempLetter))            //Same letter
                        {
                            if (parentRow.transform.GetChild(i).GetComponent<ChosenLetter>().correctPlace)
                            {
                                Debug.Log("Returning");
                                return;
                            }
                        }
                    }
                }

                letterBG.color = Color.yellow;
            }

            //Update Color also accordingly
            /*if (!letterChosen.Equals('0'))
            {
                char letter = '0';
                //loop through to check if the chosen letter is in the currentWord
                for (int i = 0; i < localGameLogic.currentWord.Length ; i++)
                {
                    letter = localGameLogic.currentWord[i];
                    if (letterChosen.Equals(letter))
                    {
                        //Check index of the letter
                        if (letterIndex == i)
                        {

                        }
                    }
                }
            }*/
        }

        private void UpdateAnimateLetterIndex()
        {
            if (localGameLogic.animateLetterIndex >= 4)
            {
                localGameLogic.animateLetterIndex = 0;          // Reset for next row
                GameManager.instance.currentLetterIndex = 0;
                GameManager.instance.currentRowIndex++;
                GameManager.instance.correctLettersChosen = 0;
                //localGameLogic.checkLetterInvoked= false;
                //Debug.Log($"Resetting checkLetterInvoked");

                localGameLogic.CheckRowsLeft();
                return;
            }

            localGameLogic.animateLetterIndex++;
            localGameLogic.OnKeyPressed?.Invoke(255, false);
        }

        private void ResetLetter()
        {
            letterChosen = '0';
            letter_Txt.text = "";
            letterBG.sprite = bgSprites[0];
            letterBG.color = ogColor;
        }
    }
}