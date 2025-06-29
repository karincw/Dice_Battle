using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Shy
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshProUGUI tmp;
        private Sequence seq;
        private float pos = 0.3f;

        private void Awake()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            seq = DOTween.Sequence();

            seq.SetAutoKill(false);

            seq.AppendInterval(0.2f);
            seq.Append(transform.DOLocalMoveY(20, pos));
            seq.Insert(0.3f, tmp.DOFade(0, 0.2f).OnComplete(() => Pooling.Instance.Return(gameObject, PoolingType.DmgText)));

            seq.Pause();
        }

        public void Use(string _mes)
        {
            tmp.alpha = 1;
            tmp.text = _mes;

            gameObject.SetActive(true);

            seq.Restart();

            pos = 0.3f;
        }

        public void Use(string _mes, float _y)
        {
            pos += _y;
            Use(_mes);
        }
    }
}
