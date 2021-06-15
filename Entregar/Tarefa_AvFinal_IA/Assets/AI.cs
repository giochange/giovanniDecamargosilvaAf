using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player;
    public Transform bulletSpawn;
    public Slider healthBar;   
    public GameObject bulletPrefab;

    NavMeshAgent agent;
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f;// vida
    float rotSpeed = 5.0f;// rotação

    float visibleRange = 80.0f;
    float shotRange = 40.0f;//tiro

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();// agente navmash
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth",5,0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        healthBar.value = (int)health;// valor da vida
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0);// posição da barra de vida
    }

    void UpdateHealth()// atualização da vida
    {
       if(health < 100)
        health ++;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "bullet")
        {
            health -= 10;// retirada da vida
        }
    }

    [Task]
    public void PickRandomDestination()// destino randomico
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void MoveToDestination()// mover ao destino
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);

        if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void PickDestination(int x, int z)//pegada do destino
    {
        Vector3 dest = new Vector3(x, 0, z);
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void TargetPlayer()// pega a posição do player
    {
        target = transform.position;
        Task.current.Succeed();
    }

    [Task]
    public bool Fire()// atira no player se estiver dentro do range
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2000);

        return true;
    }

    [Task]
    public void LookAtTarget()// olha para o player
    {
        Vector3 direction = target - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(this.transform.forward, direction));

        if(Vector3.Angle(this.transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    bool SeePlayer()
    {
        Vector3 distance = player.transform.position - this.transform.position;
        RaycastHit hit;
        bool seeWall = false;
        Debug.DrawRay(this.transform.position, distance, Color.red);
        if (Physics.Raycast(this.transform.position, distance, out hit))
        {
            if (hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("wall={0}", seeWall);

        if (distance.magnitude < visibleRange && !seeWall)
            return true;
        else
            return false;
    }

    [Task]
    bool Turn(float angle)// vira
    {
        var p = this.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        target = p;
        return true;
    }

    [Task]
    public void Follow()// segue
    {
        agent.SetDestination(player.position);
        Task.current.Succeed();
    }

    [Task]
    public bool IsHealthLessThan(float health)
    {
        return this.health < health;
    }

    [Task]
    public bool Explode()// destroi
    {
        Destroy(healthBar.gameObject);
        Destroy(this.gameObject);
        return true;
    }
}

