using DG.Tweening;
using karin.Core;
using UnityEngine;
using UnityEngine.UI;

namespace karin
{
    public class RewardCanvas : MonoBehaviour
    {
        [SerializeField] private float _openDuration;
        [SerializeField] private float _closeDuration;

        private RewardPanel _rewardPanel;
        private SceneChangeBtn _sceneBtn;
        private Image _bg;
        private Color _bgAlphaZero;
        private readonly Vector2 offsetPanelPosition = new Vector2(0, 1500);

        private void Awake()
        {
            _rewardPanel = GetComponentInChildren<RewardPanel>();
            _bg = transform.Find("BG").GetComponent<Image>();
            _sceneBtn = GetComponentInChildren<SceneChangeBtn>();
            _bgAlphaZero = _bg.color;
            _bgAlphaZero.a = 0;
            _bg.color = _bgAlphaZero;
            _rewardPanel.transform.localPosition = offsetPanelPosition;
        }

        public void Open(bool isWin, int gem, int coin)
        {
            gameObject.SetActive(true);
            _rewardPanel.SetData(isWin, gem, coin);
            _sceneBtn.targetSceneName =  isWin ? $"WorldMap" : $"Title";

            DataLinkManager.Instance.Gem.Value += gem;
            DataLinkManager.Instance.Coin.Value += coin;

            _bg.DOFade(1, _openDuration / 2);
            _rewardPanel.Fade(0.85f, _openDuration);
            _rewardPanel.transform.DOLocalMoveY(0, _openDuration);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _bg.DOFade(0, 0);
            _rewardPanel.Fade(0, 0);
            _rewardPanel.transform.DOMoveY(offsetPanelPosition.y, 0);
        }
    }
}
