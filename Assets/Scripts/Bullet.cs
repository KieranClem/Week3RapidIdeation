using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 20f;
    public Rigidbody rb;
    public Material FrozenColour;
    public float destoryTime = 10f;
    private bool canFreeze;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * Speed;
        Invoke("DestroyTime", 0.0f);
        canFreeze = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isFeezeCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && canFreeze)
        {
            Debug.Log(other.name);
            other.GetComponent<EnemyFrozen>().isFrozen = true;
            other.GetComponent<MeshRenderer>().material = FrozenColour;
        }

        Destroy(gameObject);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(destoryTime);

        Destroy(gameObject);
    }
}
