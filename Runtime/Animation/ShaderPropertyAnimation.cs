using UnityEngine;

namespace Animation
{
    public class ShaderPropertyAnimation : MonoBehaviour
    {
        private float _timer;
        private float _duration;
        private bool _isActive;
        private Color _colorFrom;
        private Color _colorTo;
        private float _floatFrom;
        private float _floatTo;
        private string _propertyName;
        private bool _isBack;
        private int _type;

        private float _timer1;
        private float _timer2;
        
        public void AnimateColorProperty(string name, Color from, Color to, float time)
        {
            _isActive = true;
            _duration = time;
            _timer = 0;
            _colorFrom = from;
            _colorTo = to;
            _propertyName = name;
            _isBack = false;
            _type = 1;
        }

        public void AnimateFloatProperty(string name, float from, float to, float time)
        {
            _isActive = true;
            _duration = time;
            _timer = 0;
            _floatFrom = from;
            _floatTo = to;
            _propertyName = name;
            _isBack = false;
            _type = 0;
            _timer1 = 0;
            _timer2 = 0;
        }
        
        protected virtual void Update()
        {
            if (!_isActive)
            {
                return;   
            }
            
            _timer += Time.deltaTime;
            if (_timer >= _duration / 2f)
            {
                _isBack = true;
                _timer2 += Time.deltaTime;
            }
            else
            {
                _timer1 += Time.deltaTime;
            }

            if (_timer > _duration)
            {
                _isActive = false;
            }

            if (_type == 0)
            {
                var c = _isBack ? Mathf.Lerp(_floatTo, _floatFrom, _timer2 / _duration * 2) : Mathf.Lerp(_floatFrom, _floatTo, _timer1 / _duration * 2);
                GetComponent<Renderer>().material.SetFloat(_propertyName, c);
            }
            else
            {
                var c = _isBack ? Color.Lerp(_colorTo, _colorFrom, _timer / _duration * 2) : Color.Lerp(_colorFrom, _colorTo, _timer / _duration * 2);
                GetComponent<Renderer>().material.SetColor(_propertyName, c);
            }
        }
    }
}