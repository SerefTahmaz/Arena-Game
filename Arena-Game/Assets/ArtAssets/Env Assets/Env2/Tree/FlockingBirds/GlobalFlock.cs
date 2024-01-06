using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {

    public GlobalFlock m_MyFlock;
    public GameObject m_BirdPrefab;
    //public static int tankSize = 5;

    static int m_NumBird = 50;
    public GameObject[] m_AllBird = new GameObject[m_NumBird];
    public Vector3 m_GoalPos;

    //public static Vector3 goalPos = new Vector3(308.75f, 6.23f, 1275.9f);
    public Vector3 m_SwimLimits = new Vector3(5, 5, 5);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(m_SwimLimits.x * 2, m_SwimLimits.y * 2, m_SwimLimits.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(m_GoalPos, 0.1f);
    }
    void Start()
    {
        m_MyFlock = this;
        m_GoalPos = transform.position;
        for (int i=0; i < m_NumBird; i++)
        {
            Vector3 pos = this.transform.position+ new Vector3(Random.Range(-m_SwimLimits.x,m_SwimLimits.x),
                                        Random.Range(-m_SwimLimits.y, m_SwimLimits.y),
                                        Random.Range(-m_SwimLimits.z, m_SwimLimits.z));
            m_AllBird[i] = (GameObject)Instantiate(m_BirdPrefab, pos, Quaternion.identity);
            m_AllBird[i].GetComponent<flock>().myManager = this;
        }
        
    }
    // Update is called once per frame
    void Update () {
		if(Random.Range(0,10000)<50)
        {
            m_GoalPos = this.transform.position + new Vector3(Random.Range(-m_SwimLimits.x, m_SwimLimits.x),
                                        Random.Range(-m_SwimLimits.y, m_SwimLimits.y),
                                        Random.Range(-m_SwimLimits.z, m_SwimLimits.z));
        }
	}
}
