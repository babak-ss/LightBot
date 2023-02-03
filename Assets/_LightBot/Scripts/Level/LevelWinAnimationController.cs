using System;
using LightBot.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LightBot
{
    public class LevelWinAnimationController : MonoBehaviour
    {
        private bool _isWinAnimation = false;
        private float _winAnimationTimer = 0;
        private const float WIN_TIME = 2;
        private Image _panelImage;
        
        [SerializeField] private VoidEventSO _levelWonEvent;

        private void Start()
        {
            _levelWonEvent.Subscribe(OnLevelWonEventListener);
            _panelImage = GetComponent<Image>();
            ResetAnimation();
        }

        private void OnDisable()
        {
            ResetAnimation();
        }

        private void OnLevelWonEventListener()
        {
            _isWinAnimation = true;
        }

        private void Update()
        {
            if (_isWinAnimation)
            {
                _winAnimationTimer += Time.deltaTime;

                _panelImage.color = new Color(_panelImage.color.r, 
                                            _panelImage.color.g,
                                            _panelImage.color.b, 
                                            _panelImage.color.a + Time.deltaTime * 3);

                if (_winAnimationTimer > WIN_TIME)
                {
                    ResetAnimation();
                }
            }
        }

        private void ResetAnimation()
        {
            _isWinAnimation = false;
            _winAnimationTimer = 0;
            _panelImage.color = new Color(_panelImage.color.r,
                _panelImage.color.g,
                _panelImage.color.b,
                0);
        }
    }
}