using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Wordle_Clone
{
    public class GameLogic : MonoBehaviour
    {
        public Action<byte, bool> OnKeyPressed;
        public Action OnParticleSystemCalled, OnGameWon, OnGameRestart, OnGameLost;
        public Action<byte, bool, bool> OnLetterRepeat;

        [SerializeField] private string _currentWord;
        public string currentWord { get { return _currentWord; } }

        //private char[] _currentWord_Arr = new char[5];
        //public char[] currentWord_Arr { get { return _currentWord_Arr; } }

        public byte animateLetterIndex;
        [SerializeField] private GameObject loadingPanel;

        [Header("API Stuff")]
        [SerializeField] private string url;
        [SerializeField] private HandleApiCalls localHandleApi;

        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem confettiBurstRadial;
        [SerializeField] private ParticleSystem confettiBurstCone;

        [Header("Reference Scripts")]
        [SerializeField] private MainCanvas localMainCanvas;

        public string enteredWord;
        private bool[] letterRepeated;
        //public bool checkLetterInvoked;

        public int[][] checkLetterRepeat;
        public Dictionary<byte, byte> checkLetterRepeat2;

        private void OnEnable()
        {
            OnParticleSystemCalled += FireParticles;
        }

        private void OnDisable()
        {
            OnParticleSystemCalled -= FireParticles;
        }

        private void Start()
        {
            letterRepeated = new bool[5];

            checkLetterRepeat2 = new Dictionary<byte, byte>();

            checkLetterRepeat = new int[5][];

            for (int i = 0; i < 5; i++)
                checkLetterRepeat[i] = new int[2];
        }

        //On the StartGame button, under the StartPanel
        public void StartGame()
        {
            //_currentWord = GetWord();              //Uncomment for real gameplay

            localMainCanvas.PlayLoadingAnimation(false);

            localHandleApi.GetOnlineWord(ref url);
            InvokeRepeating(nameof(GetWord), 0f, 1f);

            //for (int i = 0; i < currentWord.Length; i++)
            //    _currentWord_Arr[i] = currentWord[i];
        }

        //Get the word through API
        public void GetWord()
        {
            if (localHandleApi.currentWord != null)
            {
                CancelInvoke(nameof(GetWord));
                _currentWord = localHandleApi.currentWord.word;
                _currentWord = _currentWord.ToUpper();
                localMainCanvas.PlayLoadingAnimation(true);
                loadingPanel.SetActive(false);
            }
            //OnlineWord onlineWord = 
            //string currentWord = onlineWord.word;
            //currentWord.ToUpper();
            //return currentWord;
        }

        private void FireParticles()
        {
            confettiBurstRadial.Play();
            confettiBurstCone.Play();
        }

        public void CheckLetter()
        {
            #region Attempt1
            //if (!checkLetterInvoked)
            {
                //checkLetterInvoked = true;

                //chcek for word
                /*Debug.Log($"entered owrd : {enteredWord}, got word : {currentWord}");


                for (int i = 0; i < 5; i++)
                {
                    if (enteredWord[i].Equals(currentWord[i]))
                    {
                        OnLetterRepeat?.Invoke((byte)i, true, true);
                        Debug.Log($"repeat. INdex : {i}");
                    }
                    else
                    {
                        //Debug.Log($"{i} : {letterIndex}");
                        byte letterValue = (byte)enteredWord[i];
                        for (int j = 0; j < 5; j++)
                        {
                            if (enteredWord[i].Equals(enteredWord[j]))
                            {
                                //OnLetterRepeat?.Invoke((byte)i, true);
                                if (checkLetterRepeat2.TryGetValue(letterValue, out byte value))
                                {
                                    if (value == 0)
                                    {
                                        OnLetterRepeat?.Invoke((byte)i, true, false);
                                        checkLetterRepeat2[letterValue]++;
                                    }
                                    else
                                    {
                                        Debug.Log("Increasing value");
                                        checkLetterRepeat2[letterValue]++;
                                        OnLetterRepeat?.Invoke((byte)i, false, false);
                                    }
                                }
                            }
                        }
                    }
                }

                string keysInDictionary = "";
                foreach (KeyValuePair<byte, byte> kvp in checkLetterRepeat2)
                {
                    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    keysInDictionary += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                }
                Debug.Log($"value : {keysInDictionary}");*/

                /*for (int i = 0; i < 5; i++)
                {
                    checkLetterRepeat[i][0] = enteredWord[i];

                    for (int j = 0; j < 5; j++)
                    {

                        //OnLetterRepeat?.Invoke((byte)i, true);

                        char letter = '0';
                        if (enteredWord[i].Equals(currentWord[i]))
                        {
                            OnLetterRepeat?.Invoke((byte)i, true, true);
                            //Debug.Log($"repeat. INdex : {i}, j: {j}");

                        }
                        else if (enteredWord[i].Equals(enteredWord[j]))
                        {
                            if (checkLetterRepeat[i][1] == 1)                   //BEcause it will also count itself
                                OnLetterRepeat?.Invoke((byte)i, true, false);

                            checkLetterRepeat[i][1]++;
                            //OnLetterRepeat?.Invoke((byte)i, true);

                        }

                        //loop through to check if the chosen letter is in the currentWord
                        //for (int x = 0; x < currentWord.Length; x++)
                        //{
                        //    if (enteredWord[i].Equals(letter))
                        //    {
                        //        //Check index of the letter
                        //        if (i == x)
                        //        {
                        //            OnLetterRepeat?.Invoke((byte)i, true, true);
                        //            //Debug.Log($"repeat. INdex : {i}, j: {j}");
                        //        }
                        //        else
                        //        {
                        //            if (enteredWord[i] == enteredWord[j])
                        //            {
                        //                //Debug.Log($"Not repeat. INdex : {i}, j : {j}");
                        //            }
                        //        }
                        //    }
                        //}

                    }
                }*/

                /*for (int i = 0; i < 4; i++)
                {
                    Debug.Log($"{i} : {checkLetterRepeat[i][1]}\n");
                }*/

                /*for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        checkLetterRepeat[i][j] = 0;
                    }
                }*/

            }
            #endregion Attempt1

            #region Attempt2
            for (int i = 0; i < 5; i++)
            {
                if (enteredWord[i].Equals(currentWord[i]))
                {
                    //if (!letterRepeated[i])               //Would not matter as it is in correct place
                        OnLetterRepeat?.Invoke((byte)i, true, true);

                    //if (checkLetterRepeat2.TryGetValue((byte)enteredWord[i], out byte tempValue))
                        checkLetterRepeat2[(byte)enteredWord[i]]++;
                    Debug.Log($"repeat. INdex : {i}");
                }
                else
                {
                    //Check if in word
                    //for (int j = 0; j < 5; j++)
                    {
                        //if (i != j)                         //Exclude the 1 with which refer
                        if (currentWord.Contains(enteredWord[i]))                         //Exclude the 1 with which refer
                        {
                            //Then it is present
                            //Then yellow will come
                            //Debug.Log($"Contains word : {enteredWord[i]}");

                            //Check if repeating
                            #region RepeatAttempt1
                            /*for (int j = 0; j < 5; j++)
                            {
                                if (i != j)
                                {
                                    //Check if this is the 1st time to encounter the letter
                                    if (enteredWord[i].Equals(enteredWord[j]) && !letterRepeated[i])
                                    {
                                        //Then it is repeating
                                        //This will be encountered somewhere for the 1st time.
                                        letterRepeated[i] = true;

                                        //Send the chosenLetter at index j that it comes as the first repeat letter
                                        OnLetterRepeat?.Invoke((byte)i, true, false);
                                    }
                                }
                            }*/
                            #endregion RepeatAttempt1

                            #region RepeatAttempt2
                            byte tempValue = 0;

                            for (int j = 0; j < 5; j++)
                            {
                                //Dont check at the same index
                                //if (i != j)
                                {
                                    if (enteredWord[i].Equals(enteredWord[j]))
                                    {
                                        //Access dixtionary to check what is the repeat value of the letter currently
                                        if (checkLetterRepeat2.TryGetValue((byte)enteredWord[i], out tempValue))
                                        {
                                            //Check if this is the 1st time to encounter the letter
                                            if (tempValue == 0)
                                            {
                                                OnLetterRepeat?.Invoke((byte)j, true, false);
                                                checkLetterRepeat2[(byte)enteredWord[i]]++;
                                            }
                                        }
                                    }
                                }
                                //else
                                //{
                                //    OnLetterRepeat?.Invoke((byte)i, true, false);
                                //    checkLetterRepeat2[(byte)enteredWord[i]]++;
                                //}
                            }
                            #endregion RepeatAttempt2
                        }
                        else
                        {
                            //Not present
                            //Do Nothing
                        }
                    }
                }
            }
            #endregion Attempt2

            #region Attempt3

            /*for (int i = 0; i < 5; i++)
            {
                if (enteredWord[i].Equals(currentWord[i]))
                {
                    //if (!letterRepeated[i])               //Would not matter as it is in correct place
                    OnLetterRepeat?.Invoke((byte)i, true, true);

                    //if (checkLetterRepeat2.TryGetValue((byte)enteredWord[i], out byte tempValue))
                    //checkLetterRepeat2[(byte)enteredWord[i]]++;
                    //Debug.Log($"repeat. INdex : {i}");
                }
                else
                {
                    int[] tempArray = new int[5] { -1, -1, -1, -1, -1 };

                    int firstOccurence;
                    if (currentWord.Contains(enteredWord[i]))
                    {
                        firstOccurence = currentWord.IndexOf(enteredWord[i]);
                        OnLetterRepeat?.Invoke((byte)firstOccurence, true, false);
                    }
                }
            }*/
            #endregion Attempt3

            //REset everything
            enteredWord = "";
            //for (int i = 0; i < letterRepeated.Length; i++)
            //    letterRepeated[i] = false;

            checkLetterRepeat2.Clear();
        }

        public void CheckRowsLeft()
        {
            if (GameManager.instance.currentRowIndex >= 4)
            {
                if (!GameManager.instance.gameWon)
                    OnGameLost?.Invoke();
            }
        }

        //On the Yes button, under the ExitPanel/GamePanel/MainCanvas
        public void RestartGame()
        {
            GameManager.instance.currentLetterIndex = 0;
            GameManager.instance.currentRowIndex = 0;
            GameManager.instance.correctLettersChosen = 0;
            GameManager.instance.gameWon = false;

            localHandleApi.ResetCurrentWord();
            _currentWord = "";
            animateLetterIndex = 0;

            OnGameRestart?.Invoke();
        }
    }
}