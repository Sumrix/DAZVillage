using System;

[Serializable]
public class CharacterStats :
    Stats
{
    // Атрибуты
    public float Strength;
    public float Agility;
    public float Intelligence;
    // Параметры
    public RegenerablePoints Health;
    public float Damage;
    public float Armor;
    public float MoveSpeed;
    /// <summary>
    /// Увеличение Скорости Атаки
    /// Increased Attack Speed
    /// Attacks per second = [(100 + IAS) × 0.01] / BAT
    /// Attack time = BAT / [(100 + IAS) × 0.01] = 1 / (attacks per second)
    /// </summary>
    public float IAS;
    public float Evasion;
    public float Accuracy;

    public CharacterStats()
    {
        Health = new RegenerablePoints();
    }
    public static CharacterStats operator +(CharacterStats operand1, CharacterStats operand2)
    {
        return Operate(operand1, operand2, (a, b) => a + b);
    }
    public static CharacterStats operator -(CharacterStats operand1, CharacterStats operand2)
    {
        return Operate(operand1, operand2, (a, b) => a - b);
    }
    public static CharacterStats operator *(CharacterStats operand1, float operand2)
    {
        return Operate(operand1, (a) => a * operand2);
    }
}