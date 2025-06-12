using UnityEngine;

public class AdditionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer, int wave)
    {
        int max = Mathf.RoundToInt(10 + Mathf.Log(wave + 1) * 3);  // Smooth scaling
        int a = Random.Range(1, max);
        int b = Random.Range(1, max);
        answer = Random.Range(0, 2) == 0 ? a : b;
        int result = a + b;

        if (answer == a)
            equationStr = "_ + " + b + " = " + result;
        else
            equationStr = a + " + _ = " + result;
    }
}
