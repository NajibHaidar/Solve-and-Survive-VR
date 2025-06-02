using UnityEngine;

public class SubtractionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer)
    {
        int a = Random.Range(5, 15);
        int b = Random.Range(1, a);
        answer = Random.Range(0, 2) == 0 ? a : b;
        int result = a - b;

        if (answer == a)
            equationStr = "_ - " + b + " = " + result;
        else
            equationStr = a + " - _ = " + result;
    }
}
