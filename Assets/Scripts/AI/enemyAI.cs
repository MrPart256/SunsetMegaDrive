using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class enemyAI : MonoBehaviour
{
    [Header("Статы бота")]
    public int health;
    public bool meeleEnemy;
    public bool rangeEnemy;
    private Transform player;
    private NavMeshAgent agent;

    [Header("Обнаружение игрока")]
    public LayerMask playerMask;
    public float visionLenght;
    public bool spotted = false;
    private Vector3 lastPosition;

    [Header("Поиск пути")]
    private Vector3 startPosition;

    [Header("Стрельба")]
    public float shootingRange;
    public float timeToShoot;
    public Transform shootPosition;
    public float repeatTime;
    public float time;
    private float timer;
    public float avoidRangeForShooter;
    private Vector3 randomPointForShooter;

    [Header("Ближний бой")]
    public Transform attackPoint;
    public float attackRange;
    public int damage;
    public GameObject attackEffect;
    /*
    [Header("Уворот от пуль")]
    public GameObject avoidanceArea;
    public float avoidRange;
    private Transform target;
    */

    // Use this for initialization
    void OnEnable()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        timer = timeToShoot;
        startPosition = transform.position;
        lastPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        playerDetection();
        die();
        if (meeleEnemy == true) {
            _meeleEnemy();
        }
        if (rangeEnemy == true)
        {
            _rangeEnemy();
        }


    }
    private void _meeleEnemy()
    {
        chasePlayer();
        meleeAttack();
    }
    private void _rangeEnemy()
    {
        if(player != null) { 
        if (Vector3.Distance(transform.position, player.position) < shootingRange)
        {
                
                moveAround();
            shoot();
        }
        else
        {
            chasePlayer();
        }
        }
    }
    private void OnDisable()
    {
        spotted = false;
    }
    private void playerDetection()
    {
      
        if (player != null && this.gameObject.activeSelf==true) {
           
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); //Создает луч,который находит игрока, если тот в зоне видимости
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(transform.position, (player.position - transform.position));
            if (distanceToPlayer < visionLenght)
            {
               
                if (Physics.Raycast(ray, out hit))
                {
                  
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("Нашел");
                        spotted = true;
                        
                    }
                    else
                    {
                        lastPosition = hit.point;
                        spotted = false;
                    }
                }
            }
            else { 
                spotted = false; 
            }
        }

    }
    private void chasePlayer()
    {
        if(this.gameObject.activeSelf == true) { 
        if (player != null && spotted==true) {
                LookAtTarget();
                agent.SetDestination(player.position);

        }
        else if (lastPosition != Vector3.zero)
        {
            agent.SetDestination(lastPosition);
           lastPosition = Vector3.zero;
        }
        else
        {
            agent.SetDestination(startPosition);
        }
        }
    }
    //private void avoidBullets()
    //{
    //    RaycastHit leftHit = new RaycastHit();
    //    RaycastHit rightHit = new RaycastHit();
    //    Ray leftRay = new Ray(transform.position, new Vector3(transform.position.x-2,transform.position.y,transform.position.z));
    //    Ray rightRay = new Ray(transform.position, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z));
    //    if (Physics.Raycast(leftRay,out leftHit)) //Проверка есть ли чота слева
    //    {
    //        if (leftHit.collider.gameObject.tag == "Enviroment") //Уворот направо
    //        {
    //            Debug.Log("Есть слева");
    //            agent.SetDestination(new Vector3(transform.position.x + avoidRange, transform.position.y, transform.position.z));
    //        }
    //    }
    //    else if (Physics.Raycast(rightRay, out rightHit)) //Наоборот
    //    {
    //        if (rightHit.collider.gameObject.tag == "Enviroment") //Уворот налева
    //        {
    //            Debug.Log("Есть справа");
    //            agent.SetDestination(new Vector3(transform.position.x - avoidRange, transform.position.y, transform.position.z));
    //        }
    //    }
    //    /*Здесь проблема, противник
    //     не хочет определять, что с двух сторон его что-то окружает, он просто упирается в итоге в стену, надо будет подумать*/
    //   else if (Physics.Raycast(leftRay, out leftHit) && Physics.Raycast(rightRay, out rightHit)) 
    //    {
    //        Debug.Log("С двух сторон");
    //        if (leftHit.collider.gameObject.tag == "Enviroment" && rightHit.collider.gameObject.tag == "Enviroment")
    //        {
    //            agent.SetDestination(new Vector3(transform.position.x + avoidRange, transform.position.y, transform.position.z+avoidRange));
    //        }
    //    }
    //    /*Конец проблемы*/
    //    else
    //    {
    //        Debug.Log("Ничего не мешает");
    //        agent.SetDestination(new Vector3(transform.position.x+UnityEngine.Random.Range(-avoidRange,avoidRange),transform.position.y,transform.position.z+UnityEngine.Random.Range(-avoidRange, avoidRange)));
    //    }
    //}
    private void meleeAttack()
    {
        if (player != null) {
            LookAtTarget();
        Collider[] hitPlayer= Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        if (Time.time > repeatTime && Vector3.Distance(transform.position,player.position)<attackRange)
        {
            Instantiate(attackEffect, attackPoint.position,Quaternion.identity);
            player.GetComponent<PlayerControl>().getDamage(damage);
                    Debug.Log("Ударил");
                    repeatTime = Time.time + time;
            }
        }
    }
    private void moveAround()
    {
        if (timer > timeToShoot && spotted == true && player != null)
        {
            agent.updateRotation = false;

            randomPointForShooter = new Vector3(
              Random.Range(Random.Range(player.position.x - shootingRange, player.position.x - avoidRangeForShooter), Random.Range(player.position.x + avoidRangeForShooter, player.position.x + shootingRange)), transform.position.y,
              Random.Range(Random.Range(player.position.z - shootingRange, player.position.z - avoidRangeForShooter), Random.Range(player.position.z + avoidRangeForShooter, player.position.z + shootingRange))
                );
            if (Vector3.Distance(player.position, randomPointForShooter) > avoidRangeForShooter)
            {
                agent.SetDestination(randomPointForShooter);
            }
            else
            {
                randomPointForShooter = new Vector3(
              Random.Range(Random.Range(player.position.x - shootingRange, player.position.x - avoidRangeForShooter), Random.Range(player.position.x + avoidRangeForShooter, player.position.x + shootingRange)), transform.position.y,
              Random.Range(Random.Range(player.position.z - shootingRange, player.position.z - avoidRangeForShooter), Random.Range(player.position.z + avoidRangeForShooter, player.position.z + shootingRange))
                );
            }
            timer = 0;
        }
    }
    private void shoot()
    {
       if(player!=null && spotted == true) {
            LookAtTarget();
            gameObject.GetComponentInChildren<weapon>().shoot();
            if (gameObject.GetComponentInChildren<weapon>().magazineCurrent == 0)
            {
                gameObject.GetComponentInChildren<weapon>().reload();
            }
               // transform.LookAt(player);
            time = 0;
        }
    }
    private void LookAtTarget()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,1);
    }
    public void getDamage(int damage)
    {
        health -= damage;
    }
    private void die()
    {
        if (health <= 0)
        {
            gameObject.GetComponentInChildren<weapon>().drop();
            gameObject.SetActive(false);
            // Destroy(this.gameObject);
        }
    }
  
}
