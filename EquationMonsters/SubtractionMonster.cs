using UnityEngine;

public class SubtractionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer, int wave)
    {
        int min = Mathf.RoundToInt(5 + Mathf.Log(wave + 1));
        int max = Mathf.RoundToInt(15 + Mathf.Log(wave + 1) * 2);
        int a = Random.Range(min, max);
        int b = Random.Range(1, a);
        answer = Random.Range(0, 2) == 0 ? a : b;
        int result = a - b;

        if (answer == a)
            equationStr = "_ - " + b + " = " + result;
        else
            equationStr = a + " - _ = " + result;
    }
}
