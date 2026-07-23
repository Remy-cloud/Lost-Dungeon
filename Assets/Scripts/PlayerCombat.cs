using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerAnimationJuice animJuice;

    void Awake()
    {
        animJuice = GetComponent<PlayerAnimationJuice>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animJuice?.PlayAttackJuice();
        // Actual damage-dealing to enemies will be added once we build the Health/IDamageable system
        Debug.Log("Player attacked!");
    }
}
