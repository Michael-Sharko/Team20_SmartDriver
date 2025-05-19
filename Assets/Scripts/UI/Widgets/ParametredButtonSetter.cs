using Assets.Scripts.UI.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Widgets
{
    public class ParametredButtonSetter : MonoBehaviour
    {
        [SerializeField] private ParametredButtonsSO config;

        private void Awake()
        {
            var parametredButtons = FindObjectsByType<ParametredButtonTag>
                (FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var parametredButton in parametredButtons)
            {
                var button = parametredButton.GetComponent<Button>();

                SetHighlightedColor(button);

                button.onClick.AddListener(() => PlaySound2D.Play(config.ClickSound.GetClip()));

                SetHighlightedSound(button);
            }
        }

        private void SetHighlightedColor(Button button)
        {
            var colorBlock = button.colors;
            colorBlock.highlightedColor = config.HighlightedColor;
            button.colors = colorBlock;
        }

        private void SetHighlightedSound(Button button)
        {
            var trigger = button.gameObject.GetComponent<EventTrigger>();
            trigger.triggers[0].callback.AddListener((x) =>
            {
                if (button.interactable)
                    PlaySound2D.Play(config.HighlightedSound.GetClip());
            });
        }

#if UNITY_EDITOR
        [ContextMenu("SelectAllParametredButtonInSceneView")]
        private void SelectAllParametredButtonInSceneView()
        {
            var parametredButtons = FindObjectsByType<ParametredButtonTag>
                (FindObjectsInactive.Include, FindObjectsSortMode.None);
            var gameobjects = new GameObject[parametredButtons.Length];
            for (int i = 0; i < parametredButtons.Length; i++)
            {
                gameobjects[i] = parametredButtons[i].gameObject;
            }
            UnityEditor.Selection.objects = gameobjects;
        }
#endif
    }
}