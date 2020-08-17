using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UIHealthBar : MonoBehaviour
    {
        public GameObject Target;
        public string TargetName;
        public string TargetTag;

        private Text _value;
        private float _startWidth;
        private float _startHeight;
        private bool _isHidden = true;
        
        private void Start()
        {
            _value = gameObject.GetChildByName("Value").GetComponent<Text>();

            _startWidth = gameObject.GetChildByName("HealthBar").GetComponent<RectTransform>().sizeDelta.x;
            _startHeight = gameObject.GetChildByName("HealthBar").GetComponent<RectTransform>().sizeDelta.y;
        }
        
        private void Update()
        {
            if (!Target)
            {
                Hide();
                if (TargetName != null)
                {
                    Target = GameObject.Find(TargetName);
                }
                return;
            }

            Show();
            
            _value.text = (int)Target.GetComponent<Damagable>().health + "/" + (int)Target.GetComponent<Damagable>().maxHealth;
            
            // Set health bar
            gameObject.GetChildByName("HealthBar").GetComponent<RectTransform>().sizeDelta = 
                new Vector2(Target.GetComponent<Damagable>().Percentage * _startWidth, _startHeight);
        }

        private void Hide()
        {
            _isHidden = true;

            //GetComponent<Image>().enabled = false;
            gameObject.GetChildByName("HealthBar").GetComponent<Image>().enabled = false;
            gameObject.GetChildByName("Value").GetComponent<Text>().enabled = false;
        }

        private void Show()
        {
            _isHidden = false;
            
            //GetComponent<Image>().enabled = true;
            gameObject.GetChildByName("HealthBar").GetComponent<Image>().enabled = true;
            gameObject.GetChildByName("Value").GetComponent<Text>().enabled = true;
        }
    }
}