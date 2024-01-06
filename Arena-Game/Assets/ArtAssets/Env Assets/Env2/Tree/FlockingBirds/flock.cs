using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour {

    public GlobalFlock myManager;
    public float speed = 0.001f;
    float rotationSpeed = 5.0f;
    float minSpeed = 0.8f;
    float maxSpeed = 2.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 3.0f;
    //public Vector3 newGoalPos;

    public bool turning = false;

	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed,maxSpeed);
        
	}

    /*void OnTriggerEnter(Collider other)
    {
        if(!turning)
        {
            newGoalPos = this.transform.position - other.gameObject.transform.position;
        }

        turning = true;
    }

    void OnTriggerExit(Collider other)
    {
        turning = false;
    }*/

    // Update is called once per frame
    void Update () {

        Bounds b = new Bounds(myManager.transform.position, myManager.m_SwimLimits * 2);

        if(!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if(turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(minSpeed, maxSpeed);
            
        }
        else
        {
            if (Random.Range(0, 10) < 1)
                ApplyRules();
        }
        
        transform.Translate(0, 0, speed * Time.deltaTime);
	}

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.m_AllBird;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = myManager.m_GoalPos;
        
        float dist;

        int groupSize = 0;
        foreach( GameObject go in gos)
        {
            if(go !=this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist<= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(dist<2.0f)
                    {
                        vavoid += (this.transform.position - go.transform.position);
                         
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
        
    }
}
