using System;
using System.Collections.Generic;
using UnityEngine;

using Fumiko.Systems.Debug;
using Fumiko.UI;
using Fumiko.Systems.Input;

using UnityEngine.SceneManagement;

namespace Alakajam13th
{
    public class Game : MonoBehaviour
    {
        public static Game instance;

        public int progress = 0;

        public GameObject gameOverMusic;
        public Attributes playerAttributes;
        public SimpleCameraController playerController;

        private void Update()
        {
            if (playerAttributes.health <= 0)
            {
                DynamicUISystem.instance.showUIElement(UIElements.GAME_TITLE);
                DynamicUISystem.instance.protectUIElement(UIElements.GAME_TITLE, this.gameObject);

                gameOverMusic.SetActive(true);

                playerController.movementDisabled = true;

                if (InputMapSystem.instance.getButtonDown(InputCases.MENU))
                {
                    DynamicUISystem.instance.unprotectUIElement(UIElements.GAME_TITLE, this.gameObject);
                    SceneManager.LoadScene("Loading");
                }
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }
    }
}
