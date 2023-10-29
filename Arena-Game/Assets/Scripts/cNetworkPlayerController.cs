using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

public class cNetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private NetworkAnimator m_Animator;

    private NetworkVariable<MyData> m_RandomData = new NetworkVariable<MyData>(new MyData()
        {value = 1, isAlive = true, name = "hello"}
        , NetworkVariableReadPermission.Everyone
        , NetworkVariableWritePermission.Owner);

    private NetworkVariable<Vector3> m_Direction = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private struct MyData : INetworkSerializable
    {
        public int value;
        public bool isAlive;
        public FixedString128Bytes name;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref value);
            serializer.SerializeValue(ref isAlive);
            serializer.SerializeValue(ref name);
        }
    }

    public override void OnNetworkSpawn()
    {
        m_RandomData.OnValueChanged += (value, newValue) =>
        {
            Debug.Log($"{OwnerClientId} {newValue.value} {newValue.isAlive} {newValue.name}");
        };
    }

    private void Update()
    {
        m_Animator.Animator.SetFloat("Walk", m_Direction.Value.magnitude, .4f, .1f);
        
        if(!IsOwner) return;
        
        Vector3 direction =Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction.z = 1;
        if (Input.GetKey(KeyCode.S)) direction.z = -1;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        if (Input.GetKey(KeyCode.D)) direction.x = 1;
        m_Direction.Value = direction;

        if (Input.GetKeyDown(KeyCode.T))
        {
            m_RandomData.Value = new MyData()
                { value = Random.Range(0, 100), isAlive = Random.Range(-50, 50) > 0, name = "Whats up!!!" };
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_Animator.SetTrigger("Punch");
        }

        transform.position += m_Direction.Value*Time.deltaTime * m_Speed;
        transform.forward = m_Direction.Value != Vector3.zero ? m_Direction.Value : transform.forward;
    }
}
