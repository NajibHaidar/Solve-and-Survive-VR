using UnityEngine;

public class MultiplicationMonster : EquationMonster
{
    protected override void GenerateEquation(out string equationStr, out int answer)
    {
        int a = Random.Range(1, 4);
        int b = Random.Range(1, 4);
        answer = Random.Range(0, 2) == 0 ? a : b;
        int result = a * b;

        if (answer == a)
            equationStr = "_ * " + b + " = " + result;
        else
            equationStr = a + " * _ = " + result;
    }
}
