using System;
using TMPro;
using UnityEngine;

public class TipsUIItem : MonoBehaviour
{

    [SerializeField] private TMP_Text _tipsText;

    public void SetTipsMessage(string message)
    {
        if (_tipsText != null)
            _tipsText.text = message;
    }
}