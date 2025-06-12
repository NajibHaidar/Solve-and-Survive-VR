using UnityEngine;

public class DivisionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer, int wave)
    {
        int max = Mathf.RoundToInt(4 + Mathf.Log(wave + 1));  // Scales gently
        int b = Random.Range(1, max);                          // Divisor (non-zero)
        int result = Random.Range(1, max);                     // Quotient
        int a = result * b;                                    // Dividend = quotient × divisor

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
