using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody playerRb;
    public float bounceForce = 6;
    private AudioManager audioManager;
    
    
    void Start(){
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter(Collision collision){
        audioManager.Play("bounce");
        playerRb.velocity = new Vector3(playerRb.velocity.x, bounceForce, playerRb.velocity.z);
        string materialName = collision.transform.GetComponent<MeshRenderer>().material.name;
        if(materialName == "Safe (Instance)"){
            //the ball hit the safe area
            // Debug.Log("Safe");
        }
        else if(materialName == "Unsafe (Instance)"){
            // the ball hits the unsafe area and it's game over
            GameManager.gameOver = true;
            audioManager.Play("game over");
        }
        else if(materialName == "Last Ring (Instance)" && !GameManager.levelCompleted){
            // the ball hits the last ring and level completed
            GameManager.levelCompleted = true;
            audioManager.Play("win level");
        }
  }
}
