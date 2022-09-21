using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Voidling : MonoBehaviour
{
    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private HealthComponent healthComponent;

    [SerializeField]
    private Transform target, portal, player;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    [SerializeField]
    private float speed = 1, turnSpeed = 1;
 
    // Start is called before the first frame update
    void Start()
    {
        spawner = gameObject.GetComponentInParent<Spawner>();
        if (target == null)
        {
            target = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1 && target.position != transform.position)
        {
            _direction = (target.position - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            
            // turn towards target
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, turnSpeed * Time.deltaTime);

            // move towards target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        spawner.DeactivateCopy(gameObject);
        healthComponent.FullyHeal();
        target = portal;
    }

    public void TargetPlayer()
    {
        target = player;
    }
}
