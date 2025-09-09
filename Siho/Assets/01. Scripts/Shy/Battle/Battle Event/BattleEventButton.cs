using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Shy.Event
{
    public class BattleEventButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI tmp;
        public BattleEventType eventType;
        private UnityAction<BattleEventButton> clickEvent;
        internal int per { get; private set; }
        internal bool clickAble { private get; set; }

        public void Init(UnityAction<BattleEventButton> _clickEvent)
        {
            clickAble = false;
            clickEvent = _clickEvent;
            tmp.SetText("���� ���� : 0 ����");
        }

        public void SetPercent(Character[] _characters)
        {
            per = BattleEventValue.GetPercent(eventType, _characters[0], _characters[1]);
            
            CountDownText _cdt = new(tmp, "���� ���� : ", " ����");
            TextEvent _tEvent = new();
            _tEvent.endEvent = BattleEventManager.Instance.CheckEvent;

            SequnceTool.Instance.DOCountUp(_cdt, 0, per, 0.05f, _tEvent);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!clickAble) return;

            clickEvent?.Invoke(this);
        }
    }
}
