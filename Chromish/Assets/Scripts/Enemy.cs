using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public static Enemy Create(Vector3 position) {
        Transform enemyPf = Resources.Load<Transform>("Enemy");
        Transform rangedEnemyPf = Resources.Load<Transform>("RangedEnemy");
        Transform[] enemyTypes = new Transform[2] { enemyPf, rangedEnemyPf };
        

        int randomEnemySelect = Random.Range(0, enemyTypes.Length);

        Transform enemyTransform = Instantiate(enemyPf, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.SetRandomMoveSpeed();

        EnemyUI.Instance.AddEnemy(1);

        return enemy;
    }

    private HealthSystem healthSystem;
    private RangedEnemy rangedEnemy;
    private int moveSpeed;
    private bool isRanged;
    private float rotationSpeed;
    private int deathScore;
    public bool isDead;

    private float attackRange = 3f;
    private int meleeDamage = 20;
    private bool isAttacking;
    private float timeSinceLastMeleeAttack;

    private Rigidbody rb;
    private Transform targetTransform;

    private Vector3 lookAtTarget;
    private Animator animator;
    private NavMeshAgent navMeshAgent;



    private void Start() {
        timeSinceLastMeleeAttack = 0;
        isDead = false;
        isRanged = false;
        targetTransform = Player.Instance.transform;

        healthSystem = transform.GetComponent<HealthSystem>();
        rb = transform.GetComponent<Rigidbody>();
        animator = transform.Find("EnemyVisual").GetComponent<Animator>();
        navMeshAgent = transform.GetComponent<NavMeshAgent>();  
       

        healthSystem.OnDied += HealthSystem_OnDied;

        rangedEnemy = transform.GetComponent<RangedEnemy>();

        if (rangedEnemy != null ) {
            isRanged = true;
            
        }

        //moveSpeed = 5;
        rotationSpeed = 5;
        animator.SetBool("IsWalking", false);
        if (isRanged ) {
            animator.SetBool("IsWalkingBackwards", false);
            deathScore = 20;
        } else {
            deathScore = 15;
        }
        
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        isDead = true;
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;

        animator.SetBool("IsDead", true);
        animator.SetBool("IsWalking", false);
        if (isRanged) {
            animator.SetBool("IsWalkingBackwards", false);
        }
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<CapsuleCollider>().enabled = false;

        ScoreManager.Instance.AddScore(deathScore);
        
        EnemyUI.Instance.AddEnemy(-1);
        
        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        
        yield return new WaitForSeconds(20);
        Destroy(gameObject);

    }

    private void Update() {

        if (!isDead) {
            HandleLookingAt();
            HandleGroundAlignment();

            if (!isRanged) {
                timeSinceLastMeleeAttack += Time.deltaTime;
                if (!isAttacking) {
                    if (Vector3.Distance(transform.position, targetTransform.position) > attackRange - 1) {
                        HandleMovement(false);
                    } else {
                        animator.SetBool("IsWalking", false);
                    }
                    
                }
               
                if (Vector3.Distance(transform.position, targetTransform.position) <= attackRange && timeSinceLastMeleeAttack > 2 && !isAttacking) {
                   StartCoroutine(Attack());
                }
            }

            if (isRanged) {
                if (Vector3.Distance(transform.position, targetTransform.position) < rangedEnemy.backupDistance) {
                    HandleMovement(true);
                    rangedEnemy.playerInRange = true;
                } else if (Vector3.Distance(transform.position, targetTransform.position) > rangedEnemy.shootRange) {
                    HandleMovement(false);
                    rangedEnemy.playerInRange = false;
                } else {
                    rangedEnemy.playerInRange = true;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsWalkingBackwards", false);
                }
            }
        }
        
    }

    private IEnumerator Attack() {
        isAttacking = true;

        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsWalking", false);
        StartCoroutine(AttackDamage());
        timeSinceLastMeleeAttack = 0;
        yield return new WaitForSeconds(2);
        if (!isDead) {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", false);
        }
        

    }
    private IEnumerator AttackDamage() {
        yield return new WaitForSeconds(1f);
        if (Vector3.Distance(transform.position, targetTransform.position) <= attackRange + 1f && !isDead) {
           targetTransform.GetComponent<HealthSystem>().Damage(meleeDamage);

        }
        

    }

    private void HandleMovement(bool backwards) {
        Vector3 moveDir = (targetTransform.position - transform.position).normalized;
        moveDir = new Vector3(moveDir.x, 0, moveDir.z);

        if (!backwards ) {
            animator.SetBool("IsWalking", true);
            if (!isRanged) {
                animator.SetBool("IsAttacking", false);
            }

            if (isRanged) {
                animator.SetBool("IsWalkingBackwards", false);
            }
            //transform.position += moveDir * moveSpeed * Time.deltaTime;
            navMeshAgent.SetDestination(targetTransform.position);
            
        }
        if (backwards ) {
            animator.SetBool("IsWalkingBackwards", true);
            animator.SetBool("IsWalking", false);
            navMeshAgent.ResetPath();
            transform.position -= moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleLookingAt()
    {
        lookAtTarget = targetTransform.position;
        lookAtTarget = new Vector3(lookAtTarget.x, 0, lookAtTarget.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void HandleGroundAlignment()
    {
        RaycastHit hit;
        float rayLength = 2f; 
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; 

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRandomMoveSpeed() {
        moveSpeed = Random.Range(4, 9);
    }

    public Vector3 GetTarget() {
        return targetTransform.position;
    }
}
