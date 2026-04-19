using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    Vector3 m_SpawnPostion;
    [SerializeField] CustomerStation m_TargetStation;
    [SerializeField] float m_MoveSpeed;
    [SerializeField] Animator m_Animator;
    private void Start()
    {
        m_SpawnPostion = transform.position;
    }

    /// <summary>
    /// Makes customer walk to the designated customer station
    /// </summary>
    /// <param name="a_CS"></param>
    public void GotoStation(CustomerStation a_CS) 
    {
        m_TargetStation = a_CS;
        m_TargetStation.SetCustomer(this);
        StartCoroutine(MoveToPosition(m_TargetStation.transform.position + m_TargetStation.transform.forward, OnReachingStation));

    }

    IEnumerator MoveToPosition(Vector3 a_TargetPos, Action a_OnReachingTarget = null) 
    {
        Vector2 l_Pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 l_Target = new Vector2(a_TargetPos.x, a_TargetPos.z);
        m_Animator.SetTrigger("walk");
        var l_Dist = Vector2.Distance(l_Pos, l_Target);
        while (l_Dist > 0.1f) 
        {
            l_Pos = Vector2.MoveTowards(l_Pos, l_Target, m_MoveSpeed * Time.deltaTime);
            transform.position = new Vector3(l_Pos.x, 0.0f, l_Pos.y);
            l_Dist = Vector2.Distance(l_Pos, l_Target);
            yield return null;
        }
        m_Animator.SetTrigger("idle");
        transform.position = new Vector3(l_Pos.x, 0.0f, l_Pos.y);
        a_OnReachingTarget?.Invoke();
    }

    void OnReachingStation() 
    {
        m_TargetStation.CreateNewOrder();
        Debug.Log("Reached station !");
    }

    public void OnGettingOrder() 
    {
        transform.localEulerAngles = new Vector3(0, 180, 0);
        StartCoroutine(MoveToPosition(m_SpawnPostion, OnGettingBack));
    }

    void OnGettingBack()
    {
        m_TargetStation.SetCustomer(null);
        Destroy(gameObject);
    }
}
