using System;
using UnityEngine;

[Serializable]
public class CharacterStatsManager :
    LevelDependentStatsManager<CharacterStats>
{
    private const float _damageScatter = 0.2f;

    static CharacterStats ParamsFromAttrbs(CharacterStats attrbs)
    {
        return new CharacterStats
        {
            Health =
            {
                Maximum =  attrbs.Strength * 20,
                Regen = attrbs.Strength * 0.03f
            },
            Damage = attrbs.Strength,
            IAS = attrbs.Agility,
            Armor = attrbs.Agility * 0.14f
        };
    }
    protected override void Calc()
    {
        var levelBase = _levelBonus * Level + _base;
        Main = ParamsFromAttrbs(levelBase) + levelBase;
        Result = Main + ParamsFromAttrbs(Bonus) + Bonus;
    }
    /// <summary>
    /// Изменение жизни. Рекомендуется к использованию только из класса Character.
    /// </summary>
    /// <param name="value">Значение на которое жизнь изменяется</param>
    public void ChangeHealth(float value)
    {
        float coefficient = 1;
        if (value < 0)
        {
            // Учитываем значение брони
            coefficient *= 1 - 0.06f * Result.Armor / (1 + (0.06f * Mathf.Abs(Result.Armor)));
            // Учитываем разброс урона
            coefficient *= 1 + UnityEngine.Random.Range(-_damageScatter, _damageScatter);
        }
        // Изменяем жизнь
        Result.Health.Current += value * coefficient;
        _base.Health.CurrentNormalized = Result.Health.CurrentNormalized;

        OnChanged();
    }
}