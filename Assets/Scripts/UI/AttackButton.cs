using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    [RequireComponent(typeof(Image))]
    public class AttackButton :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IManagerWaiter
    {
        public Image Progress;
        public Material ActiveMaterial;
        public Material InactiveMaterial;
        private Image _background;
        private Item _lastAmmo;
        
        private void Start()
        {
            Managers.AddWaiter(this);
            _background = GetComponent<Image>();
        }
        void IManagerWaiter.Startup()
        {
            Managers.Player.AttackSkillChange += Player_AttackSkillChange;
            Managers.Inventory.CurrentAmmoChange += Inventory_CurrentAmmoChange;
        }
        private void Player_AttackSkillChange(object sender, EventArgs e)
        {
            if (Managers.Player.AttackSkill == null)
            {
                Deactivate();
            }
            else
            {
                if (Managers.Player.AttackSkill.PeriodTimer.Period < 0.3f ||
                    !Managers.Player.AttackSkill.PeriodTimer.IsEnable)
                {
                    Activate();
                }
                else
                {
                    StartProgress();
                }
            }
        }
        private void Inventory_CurrentAmmoChange(object sender, EventArgs e)
        {
            if (Managers.Player.AttackSkill != null && Managers.Inventory.CurrentAmmoType != _lastAmmo)
            {
                _lastAmmo = Managers.Inventory.CurrentAmmoType;
                if (Managers.Inventory.CurrentAmmoType == null)
                {
                    Deactivate();
                }
                else
                {
                    if (Managers.Player.AttackSkill.PeriodTimer.Period < 0.3f ||
                        !Managers.Player.AttackSkill.PeriodTimer.IsEnable)
                    {
                        Activate();
                    }
                    else
                    {
                        StartProgress();
                    }
                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (Managers.Player.AttackSkill != null)
            {
                Managers.Player.AttackSkill.Enable();
                if (Managers.Player.AttackSkill.PeriodTimer.Period > 0.3f)
                {
                    StartProgress();
                }
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Managers.Player.AttackSkill != null)
            {
                Managers.Player.AttackSkill.Disable();
            }
        }
        private void Update()
        {
            if (Managers.Player.AttackSkill != null)
            {
                Progress.fillAmount = Managers.Player.AttackSkill.PeriodTimer.Progress;
                if (Mathf.Approximately(Managers.Player.AttackSkill.PeriodTimer.Progress, 1))
                {
                    Activate();
                }
            }
        }
        private void Activate()
        {
            if (Managers.Inventory.CurrentAmmoType != null && Managers.Player.AttackSkill != null &&
                (Managers.Player.AttackSkill.PeriodTimer.Period < 0.3f || !Managers.Player.AttackSkill.PeriodTimer.IsEnable))
            {
                _background.material = ActiveMaterial;
                Progress.gameObject.SetActive(false);
            }
        }
        private void Deactivate()
        {
            _background.material = InactiveMaterial;
            Progress.gameObject.SetActive(false);
        }
        private void StartProgress()
        {
            if (Managers.Inventory.CurrentAmmoType != null && Managers.Player.AttackSkill != null &&
                Managers.Player.AttackSkill.PeriodTimer.IsEnable)
            {
                _background.material = InactiveMaterial;
                Progress.gameObject.SetActive(true);
            }
        }
    }
}