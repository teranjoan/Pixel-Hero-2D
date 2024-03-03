using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bloodParticle;


    public void Death(){
        Instantiate(bloodParticle,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
