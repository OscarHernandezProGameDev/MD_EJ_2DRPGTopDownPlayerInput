using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private GameObject arrowsPool;
    [SerializeField] private GameObject DestructionArrow;
    private float arrowLife = 1f;

   

    private void OnEnable()
    {
        transform.parent = null;
        arrowLife = 1f;
    }

    private void Update()
    {
        if (arrowLife > 0)
        {
            arrowLife -= Time.deltaTime;
        }
        else
        {
            ResetArrow();
        }
    }
    private void ResetArrow()
    {
        var tempDestruction = Instantiate(DestructionArrow,transform.position,Quaternion.identity);
        tempDestruction.transform.parent = null;
        
        transform.parent = arrowsPool.transform;
        transform.localPosition = new Vector3(0, -0.384f, 0);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            ResetArrow();
        }
    }


}
