using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UIExpBar : MonoBehaviour
    {
        public GameObject target;
        public string targetName;
        public string targetTag;
        
        private Text _value;
        private float _startWidth;
        private float _startHeight;
        private bool _isHidden = true;
        
        private void Start()
        {
            _value = gameObject.GetChildByName("Value").GetComponent<Text>();

            _startWidth = gameObject.GetChildByName("ExpBar").GetComponent<RectTransform>().sizeDelta.x;
            _startHeight = gameObject.GetChildByName("ExpBar").GetComponent<RectTransform>().sizeDelta.y;
        }
        
        private void Update()
        {
            if (!target)
            {
                Hide();
                if (targetName != null)
                {
                    target = GameObject.Find(targetName);
                }
                return;
            }

            Show();
            
            _value.text = target.GetComponent<Experienceable>().Status;
            
            // Set health bar
            gameObject.GetChildByName("ExpBar").GetComponent<RectTransform>().sizeDelta = 
                new Vector2(target.GetComponent<Experienceable>().CurrentProgress * _startWidth, _startHeight);
        }
        
        private void Hide()
        {
            _isHidden = true;

            GetComponent<Image>().enabled = false;
            gameObject.GetChildByName("ExpBar").GetComponent<Image>().enabled = false;
            gameObject.GetChildByName("Value").GetComponent<Text>().enabled = false;
        }

        private void Show()
        {
            _isHidden = false;
            
            GetComponent<Image>().enabled = true;
            gameObject.GetChildByName("ExpBar").GetComponent<Image>().enabled = true;
            gameObject.GetChildByName("Value").GetComponent<Text>().enabled = true;
        }
    }
}