using UnityEngine;

namespace Wordle_Clone
{
    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private Animator mainCanvas_AC;
        [SerializeField] private TMPro.TMP_Text wordleTxt, cloneTxt, winStatusTxt;
        private byte letterCount;
        [SerializeField] private GameObject winPanel;

        [Header("Script Reference")]
        [SerializeField] private GameLogic localGameLogic;

        private void OnEnable()
        {
            localGameLogic.OnGameWon += PlayWinAnimation;
            localGameLogic.OnGameRestart += RestartUI;
            localGameLogic.OnGameLost += GameFailed;
        }

        private void OnDisable()
        {
            localGameLogic.OnGameWon -= PlayWinAnimation;
            localGameLogic.OnGameRestart -= RestartUI;
            localGameLogic.OnGameLost -= GameFailed;
        }

        public void PlayLoadingAnimation(bool finished)
        {
            if (!finished)
                mainCanvas_AC.Play("Loading", 0);
            else
                mainCanvas_AC.SetBool("LoadingDone", true);
        }

        private void PlayWinAnimation()
        {
            mainCanvas_AC.SetBool("Win", true);
            mainCanvas_AC.Play("Collide", 0);
            //Invoke(nameof(FirstLetterClashed), 0.5f);
        }

        public void FirstLetterClashed()
        {
            InvokeRepeating(nameof(CallParticleSystem), 0f, 0.25f);
        }

        private void CallParticleSystem()
        {
            letterCount++;

            string tempString = wordleTxt.text;
            wordleTxt.text = tempString.Substring(0, tempString.Length - 1);

            tempString = cloneTxt.text;
            //Debug.Log($"letterCount : {letterCount}, string : {tempString}, length : {tempString.Length}");
            cloneTxt.text = tempString.Substring(1, tempString.Length - 1);

            localGameLogic.OnParticleSystemCalled?.Invoke();

            if (letterCount > 5)
            {
                CancelInvoke(nameof(CallParticleSystem));
                winPanel.SetActive(true);
                winStatusTxt.text = "You Win";
            }
        }

        private void GameFailed()
        {
            winPanel.SetActive(true);
            winStatusTxt.text = "You Lose";
        }

        private void RestartUI()
        {
            mainCanvas_AC.SetBool("Win", false);
            mainCanvas_AC.SetBool("LoadingDone", false);

            wordleTxt.text = "WORDLE";
            cloneTxt.text = "CLONE ";
        }

        //public void PlayGameFinishAnim()
        //{
        //    mainCanvas_AC.Play("Collide", 0);
        //}
    }
}