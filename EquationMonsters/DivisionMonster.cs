using UnityEngine;
public class DivisionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer)
    {
        int b = Random.Range(1, 10);           // Divisor (non-zero)
        int result = Random.Range(1, 13);      // Quotient
        int a = result * b;                    // Dividend = quotient × divisor

        if (Random.Range(0, 2) == 0)
        {
            answer = a;
            equationStr = "_ / " + b + " = " + result;
        }
        else
        {
            answer = b;
            equationStr = a + " / _ = " + result;
        }
    }
}
