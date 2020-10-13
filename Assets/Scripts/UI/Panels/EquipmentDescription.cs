using UnityEngine;
using UnityEngine.UI;
using Collections;

namespace GameUI
{
    public class EquipmentDescription :
        ItemDescription
    {
        [SerializeField]
        [RequiredField]
        protected CollectionView _statsView;
        private ObservableCollection<CharacterStat> _stats;
        private Equipment _currentEquipment;
        private int _currentEquipmentCount;

        private void Awake()
        {
            _stats = new ObservableCollection<CharacterStat>();
            _statsView.DataSource = _stats;
        }

        private void ShowStats()
        {
            _stats.Clear();

            var equippedEquipment = Managers.Inventory.Equipment[_currentEquipment.EquipmentType];
            if (equippedEquipment != null)
            {
                var ieSelectedItem = _currentEquipment.Bonus.Result.GetEnumerator();
                var ieEquippedItem = equippedEquipment.Bonus.Result.GetEnumerator();

                while (ieSelectedItem.MoveNext() && ieEquippedItem.MoveNext())
                {
                    float difference = ieSelectedItem.Current.Value - ieEquippedItem.Current.Value;
                    if (ieSelectedItem.Current.Key != "Health.CurrentNormalized" && difference > Mathf.Epsilon)
                    {
                        _stats.Add(new CharacterStat
                        {
                            Name = ieSelectedItem.Current.Key,
                            Main = ieSelectedItem.Current.Value,
                            Bonus = difference
                        });
                    }
                }
            }
            else
            {
                foreach (var stat in _currentEquipment.Bonus.Result)
                {
                    if (stat.Key != "Health.CurrentNormalized" && stat.Value > Mathf.Epsilon)
                    {
                        _stats.Add(new CharacterStat
                        {
                            Name = stat.Key,
                            Main = stat.Value,
                            Bonus = stat.Value
                        });
                    }
                }
            }
        }

        public override void SetItem(object obj)
        {
            var inspectorItem = obj as InspectorItem;
            if (inspectorItem != null)
            {
                _currentEquipment = (Equipment)inspectorItem.Prefab;
                _currentEquipmentCount = inspectorItem.Count;
            }
            else
            {
                _currentEquipment = (Equipment)obj;
                _currentEquipment.Bonus.Changed += Bonus_Changed;
            }

            base.SetItem(obj);

            ShowStats();
        }

        private void Bonus_Changed(object sender, System.EventArgs e)
        {
            ShowStats();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            
            _stats.Clear();
        }
    }
}