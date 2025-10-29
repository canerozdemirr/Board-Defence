using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class ItemSelectionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemAmountText;

        private string _itemName;
        private int _itemAmount;
        private Action<string> _onClicked;
        
        private readonly StringBuilder _itemAmountBuilder = new();

        public void Initialize(string itemName, int amount, Action<string> onClicked)
        {
            _itemName = itemName;
            _itemAmount = amount;
            _onClicked = onClicked;

            _button.onClick.AddListener(OnClick);
            _itemNameText.SetText(_itemName);
            _itemAmountBuilder.Clear();
        }

        public void UpdateAmount(int amount)
        {
            _itemAmount = amount;
            _itemAmountBuilder.Clear();
            _itemAmountBuilder.Append(_itemAmount);
            _itemAmountText.SetText(_itemAmountBuilder.ToString());
            _button.interactable = _itemAmount > 0;
        }

        private void OnClick()
        {
            _onClicked?.Invoke(_itemName);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
