using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Shy.Unit
{
    public class DiceUi : MonoBehaviour, IPointerClickHandler
    {
        #region Variable
        private Image visual, icon, userIcon;
        internal GameObject noUsed;

        [SerializeField] private int dNum;
        [SerializeField] private DiceSO data;
        public Character user;
        public Team team;

        private bool isDead;
        #endregion

        #region Init
        private void Awake()
        {
            visual = transform.Find("Visual").GetComponent<Image>();
            icon = transform.Find("Icon").GetComponent<Image>();
            userIcon = transform.Find("UserIcon").GetComponent<Image>();
            noUsed = transform.Find("None").gameObject;
        }
        
        public void Init(DiceSO _so, Team _team)
        {
            gameObject.SetActive(true);
            data = _so;
            visual.color = data.color;
            team = _team;
            isDead = false;
            HideDice();
        }

        private void HideDice()
        {
            visual.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            DeleteUser();
            noUsed.SetActive(false);
        }

        
        #endregion

        #region Kill
        public void KillDice() => isDead = true;

        public bool DiceDieCheck()
        {
            if (isDead) Destroy(gameObject);
            else HideDice();

            return isDead;
        }
        #endregion

        #region Roll
        public void RollDice()
        {
            transform.localScale = Vector3.one;
            visual.gameObject.SetActive(true);
            dNum = Random.Range(0, 6);
            icon.sprite = data.eyes[dNum].icon;

            //���߿� �ִϸ��̼����� �̵�
            RollFin();
        }

        private void RollFin()
        {
            icon.gameObject.SetActive(true);
            BattleManager.Instance.CheckDiceAllFin(this);
        }
        #endregion

        #region Use
        public EyeSO GetEyes() => data.eyes[dNum];

        public void SelectUser(Character _ch)
        {
            if (_ch.team != team) return;
            if (!BattleManager.Instance.CanSelectChacter(_ch)) return;

            user = _ch;
            userIcon.gameObject.SetActive(true);
            userIcon.sprite = _ch.GetIcon();
        }

        public void DeleteUser()
        {
            user = null;
            userIcon.sprite = null;
            userIcon.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            DeleteUser();
        }
        #endregion
    }
}
