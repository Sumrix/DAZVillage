using System;

[Serializable]
public class Stats :
    NumericTree
{
    public static Stats operator +(Stats operand1, Stats operand2)
    {
        return Operate(operand1, operand2, (a, b) => a + b);
    }
    public static Stats operator -(Stats operand1, Stats operand2)
    {
        return Operate(operand1, operand2, (a, b) => a - b);
    }
    public static Stats operator *(Stats operand1, float operatod2)
    {
        return Operate(operand1, x => x * operatod2);
    }
}