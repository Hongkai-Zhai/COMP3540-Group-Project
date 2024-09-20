using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI messageText;
    public float messageDuration = 4f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.PickUpKey();

                Destroy(gameObject);
            }
        }
    }
   
}
