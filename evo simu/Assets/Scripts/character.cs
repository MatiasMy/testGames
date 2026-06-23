using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    private float speed = 5;
    private Vector3 target;
    private bool foundsomething = false;
    public CircleCollider2D col;
    private float colsize;
    private float sight = 50f;
    private float hunger = 100f;
    public int gender = 1;
    private bool looking = false;
    public bool lookinForPartner;
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        colsize = col.radius;
    }


    void Update()
    {
        hunger -= Time.deltaTime;
        if (hunger > 100)
        {
            hunger = 100f;
        }
        
        if (hunger <= 0)
        {
            Destroy(gameObject);
        }

        if (hunger > 70)
        {
            lookinForPartner = true;
        }
        else
        {
            lookinForPartner = false;
        }

        if (transform.position == target)
        {
            StartCoroutine(wait());
            if (foundsomething == true)
            {
                foundsomething = false;
            }
            else
            {
                target = new Vector3(Random.Range(-37, 37), Random.Range(-20, 20));
            }
        }
        else
        {
            Debug.Log(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        IEnumerator wait()
        {
            speed = 0;
            LookAround();
            yield return new WaitForSeconds(3f);
            looking = false;
            col.radius = colsize;
            speed = 5;
        }
    }
    private void LookAround()
    {
        looking = true;
        col.radius = sight;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "food" && looking == true && hunger < 69f)
        {
            foundsomething = true;
            target = other.transform.position;
        }
        if (other.gameObject.tag == "food" && looking == false && hunger < 69f)
        {
            hunger += 50f;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "glorb" && looking == true && lookinForPartner == true && other.GetComponent<character>().lookinForPartner == true && other.GetComponent<character>().gender != gender)
        {
            Debug.Log("Found a partner");
            foundsomething = true;
            target = other.transform.position;
        }
    }
}
