using System;
using UnityEngine;

namespace LightBot
{
    public class BotAnimationController : MonoBehaviour
    {
        private bool _isLighting = false;
        private Renderer _lightRenderer;
        private float _blueValue = 0;
        private float _lightTimer = 0;
        private Vector3 initialPosition;

        void Start()
        {
            _lightRenderer = transform.GetChild(1).GetComponent<Renderer>();
        }
        
        void Update()
        {
            if (_isLighting)
            {
                _blueValue += Time.deltaTime * 2;
                if (_blueValue <= 0.5f)
                {
                    _lightRenderer.material.color = new Color(1f - _blueValue, 1f - _blueValue, 1);
                    
                    transform.position = new Vector3(transform.position.x, 
                                                    transform.position.y + Time.deltaTime / 3,
                                                    transform.position.z);
                }
                else if (_lightTimer <= 0.5f)
                {
                    _lightTimer += Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, 
                                                    transform.position.y - Time.deltaTime / 2,
                                                    transform.position.z);
                }
                else
                {
                    _isLighting = false;
                    _lightTimer = 0;
                    _blueValue = 0;
                    transform.position = initialPosition;
                    _lightRenderer.material.color = Color.white;
                }
            }
        }

        public void StartLightAnimation()
        {
            _isLighting = true;
            _lightTimer = 0;
            _blueValue = 0;
            initialPosition = transform.position;
        }
    }
}