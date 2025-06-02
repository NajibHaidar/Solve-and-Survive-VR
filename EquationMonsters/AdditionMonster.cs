using UnityEngine;

public class AdditionMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer)
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        answer = Random.Range(0, 2) == 0 ? a : b;
        int result = a + b;

        if (answer == a)
            equationStr = "_ + " + b + " = " + result;
        else
            equationStr = a + " + _ = " + result;
    }
}
