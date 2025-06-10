using UnityEngine;

public class MonsterHitbox : MonoBehaviour
{
    private EquationMonster parentMonster;

    void Start()
    {
        parentMonster = GetComponentInParent<EquationMonster>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentMonster == null) return;

        // Handle player contact
        if (other.CompareTag("Player"))
        {
            parentMonster.OnPlayerCollision(other);
        }

        // Handle answer ball contact
        if (other.CompareTag("AnswerBall"))
        {
            AnswerBall ball = other.GetComponent<AnswerBall>();
            if (ball != null && ball.HasBeenGrabbed())
            {
                parentMonster.OnAnswerHit(other);
            }
        }

    }
}
