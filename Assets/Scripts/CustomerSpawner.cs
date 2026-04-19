using System.Linq;
using UnityEngine;

/// <summary>
/// Spawns customers regularly at an interval
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] Customer[] m_CustomerPrefabs;
    [SerializeField] CustomerStation[] m_AllStations;

    [SerializeField] float m_GapBetweenCustomers;
    float m_Timer;

    private void Start()
    {
        m_Timer = 0.0f;
    }

    private void Update()
    {
        if (GetEmptyStations().Length > 0)
        {
            if (m_Timer <= 0.0f)
            {
                m_Timer = m_GapBetweenCustomers;
                SpawnCustomer(GetEmptyStations()[Random.Range(0, GetEmptyStations().Length)]);
            }
            else
                m_Timer -= Time.deltaTime;
        }
    }


    void SpawnCustomer(CustomerStation a_TargetStation) 
    {
        Customer l_C = Instantiate(m_CustomerPrefabs[Random.Range(0, m_CustomerPrefabs.Length)], transform);
        l_C.transform.localPosition = Vector3.zero;
        l_C.transform.rotation = transform.rotation;
        l_C.GotoStation(a_TargetStation);
    }

    public CustomerStation[] GetEmptyStations() 
    {
        return m_AllStations.Where(n => n.CurrentCustomer == null).ToArray();
    }
}
