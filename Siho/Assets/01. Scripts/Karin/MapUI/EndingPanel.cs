using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanel : FadeUI
{
    [SerializeField] private Image _resultImage;
    [SerializeField] private TMP_Text _resultText;

    [SerializeField] private Sprite _clearImage, _failImage;

    protected override void Awake()
    {
        base.Awake();

    }

    public void Open(bool gameResult)
    {
        _resultText.text = gameResult ? "Ŭ����!" : "����...";
        _resultImage.sprite = gameResult ? _clearImage : _failImage;
        Open();
    }   
}
