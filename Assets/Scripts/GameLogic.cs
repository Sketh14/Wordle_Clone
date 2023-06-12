using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wordle_Clone
{
    public class GameLogic : MonoBehaviour
    {
        public Action<byte> OnKeyPressed;
        public Action OnParticleSystemCalled, OnGameWon, OnGameRestart, OnGameLost;

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

        private void OnEnable()
        {
            OnParticleSystemCalled += FireParticles;
        }

        private void OnDisable()
        {
            OnParticleSystemCalled -= FireParticles;
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