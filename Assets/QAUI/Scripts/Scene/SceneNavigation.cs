using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QAUI
{
    public class SceneNavigation : MonoBehaviour
    {
        private const string UnitySceneName = "QAUI/Scene";

        [Header("UI")] 
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject content;

        [Header("Dialog")] 
        [SerializeField] private GameObject dialog;
        [SerializeField] private GameObject dialogContent;
        [SerializeField] private Button overlay;

        [Header("Navigation")] 
        [SerializeField] private Text titleText;
        [SerializeField] private Button backButton;
        [SerializeField] private Button homeButton;
        
        [Header("Scene")]
        [SerializeField] internal Scene mainScene;
        
        private static Type _currentScene;
        private static readonly Stack<Type> SceneHistory = new Stack<Type>();

        private Scene SceneComponent
        {
            get
            {
                // If a Scene component already exists, return it.
                Scene scene = GetComponent<Scene>();
                if (scene != null) return scene;

                // If the current scene is not set, set MainScene as the current scene.
                if (_currentScene == null)
                {
                    _currentScene = mainScene.GetType();
                }

                // Add a Scene component corresponding to the current scene.
                scene = gameObject.SafeAddComponent(_currentScene) as Scene;
                return scene;
            }
        }

        private void Awake()
        {
            overlay.onClick.AddListener(CloseDialog);

            titleText.text = SceneComponent.Title();
            backButton.onClick.AddListener(NavigateBack);
            homeButton.onClick.AddListener(NavigateHome);

            SceneComponent.Initialize(content);
        }

        private void NavigateBack()
        {
            if (SceneHistory.Count > 0)
            {
                _currentScene = SceneHistory.Pop();
                SceneManager.LoadScene(UnitySceneName);
            }
            else
            {
                Debug.Log("No scene history to go back to.");
            }
        }

        private void NavigateHome()
        {
            ClearHistory();

            if (_currentScene == mainScene.GetType()) return;

            _currentScene = null;
            SceneManager.LoadScene(UnitySceneName);
        }

        public static void ClearHistory() => SceneHistory.Clear();

        public static void StartScene<T>() where T : Scene => StartScene(typeof(T));

        public static void StartScene(Type sceneType)
        {
            SceneHistory.Push(_currentScene);
            _currentScene = sceneType;

            SceneManager.LoadScene(UnitySceneName);
        }

        internal void ShowDialog(IDialog dialogInstance)
        {
            dialog.SetActive(true);
            dialogInstance.Initialize(dialogContent);
        }

        internal void CloseDialog()
        {
            dialogContent.RemoveAllChildren();
            dialog.SetActive(false);
        }
    }
}