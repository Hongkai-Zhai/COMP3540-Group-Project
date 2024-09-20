using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Vector3 movement;
    private float targetYRotation;
    public GameObject projectilePrefab;
    public float planeSize = 90f;
    public Transform firePoint; 
    public bool hasKey = false;
    public TextMeshProUGUI KeyText;
    private float messageDuration = 3f;

    public Vector3 prisonCenter = new Vector3(-300, 3, -300);
    public Vector2 prisonSize = new Vector2(200, 200);
    public TextMeshProUGUI noAccessText;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        MovePlayer();

        if (movement != Vector3.zero)
        {
            targetYRotation = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(90, targetYRotation, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
        }
    }

    void MovePlayer()
    {
        Vector3 targetPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // 检查是否可以移动到目标位置
        if (CanMoveTo(targetPosition))
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, -planeSize / 2, planeSize / 2);
            targetPosition.z = Mathf.Clamp(targetPosition.z, -planeSize / 2, planeSize / 2);
            transform.position = targetPosition;
        }
        else
        {
            // 如果不能移动到目标位置，尝试沿着边界滑动
            Vector3 slideX = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
            Vector3 slideZ = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

            if (CanMoveTo(slideX))
                transform.position = slideX;
            else if (CanMoveTo(slideZ))
                transform.position = slideZ;
        }
    }

    bool CanMoveTo(Vector3 position)
    {
        // 检查是否在监狱区域内
        bool isInPrisonArea =
            position.x >= prisonCenter.x - prisonSize.x / 2 &&
            position.x <= prisonCenter.x + prisonSize.x / 2 &&
            position.z >= prisonCenter.z - prisonSize.y / 2 &&
            position.z <= prisonCenter.z + prisonSize.y / 2;

        // 如果在监狱区域内且没有钥匙，则不能移动
        if (isInPrisonArea && !hasKey)
        {
            StartCoroutine(ShowNoAccessMessage());
            return false;
        }
            

        return true;
    }

    void ShootProjectile()
    {
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        Vector3 shootDirection = transform.up;

        Quaternion spawnRotation = Quaternion.LookRotation(shootDirection);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, spawnRotation);

        projectile.transform.forward = shootDirection;

        Debug.Log("Shoot Direction: " + shootDirection);
        Debug.Log("Projectile forward: " + projectile.transform.forward);
    }

    public void PickUpKey()
    {
        hasKey = true;
        StartCoroutine(ShowKeyMessage());

    }

    private IEnumerator ShowKeyMessage()
    {
        KeyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        KeyText.gameObject.SetActive(false);

    }

    private IEnumerator ShowNoAccessMessage()
    {
        noAccessText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        noAccessText.gameObject.SetActive(false);
        
    }

}