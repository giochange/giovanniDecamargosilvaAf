using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour {

    [Header("Speed")]
	float speed = 20.0F;// variavel de velocidade
    float rotationSpeed = 120.0F;// rotação

    [Header("Bullet")]
    public GameObject bulletPrefab;// prefab da bala
    public Transform bulletSpawn;// spawn da bala

    [Header("Vida")]
    
    public GameManager manager;// herança do gameManager
   
    public float health = 100;// variavel de vida

    [Header("Collider")]
    float translationMovementValue;
    public GameObject player;// assosiação ao player
    public Transform playerCol;// transforme

    [Header("Respawn")]
    public GameObject prefab;

    [SerializeField] private Transform playerDown;
    [SerializeField] private Transform respawnPoint;

    private void Start()
    {
        InvokeRepeating("UpdateHealth", 5, 5.0f);// reload

        CheckMovement();
    }

    void Update() {
       

       

        float translation = Input.GetAxis("Vertical") * speed;// controle de movimento vertical
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;// controle de rotação
        translation *= Time.deltaTime;// multiplicação por delta time
        rotation *= Time.deltaTime;// multiplicação por deltatime para a rotação
        transform.Translate(0, 0, translation);// pegada da posição
        transform.Rotate(0, rotation, 0);//pegada da rotação

        if(Input.GetKeyDown("space"))// metodo para atirar
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);//instancia o obj bala
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward*2000);// aplicação de força para a bala atirar
        }
        
    }

   

     void OnCollisionEnter(Collision col)
     {
         if (col.gameObject.tag == "Shell")// detctação de colisão
         {
            

            TakeDamage(2f);// tira a vida
         }
     }

    public void TakeDamage(float amnt)
    {
        health -= amnt;
        if (health <= 0f)// aplica a morte do player
        {
            manager.GameOver();
        }
        float _h = Mathf.Clamp(health, 0, 100f);
        
    }

    void CheckMovement()// verificaçao de movimentação
    {
        RaycastHit hitL, hitR;

        if (Physics.Raycast(player.transform.position, Vector3.left, out hitL))
        {
            if (hitL.distance < translationMovementValue)
            {
                translationMovementValue = hitL.distance;
            }
        }

        if (Physics.Raycast(player.transform.position, Vector3.right, out hitR))
        {
            if (hitR.distance < translationMovementValue)
            {
                translationMovementValue = hitR.distance;
            }
        }
    }
}
