using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (CanAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private bool CanAttack()
    {
        return Input.GetMouseButtonDown(0)
               && cooldownTimer > attackCooldown
               && playerMovement.CanAttack();
    }

    private void Attack()
    {
        SoundManager.instance?.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        int fireballIndex = FindFireball();
        ActivateFireball(fireballIndex);
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void ActivateFireball(int index)
    {
        fireballs[index].transform.position = firePoint.position;
        Projectile projectile = fireballs[index].GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetDirection(Mathf.Sign(transform.localScale.x));
        }
        fireballs[index].SetActive(true);
    }
}