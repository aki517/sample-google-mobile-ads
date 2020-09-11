using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// .
/// </summary>
public class GamePlayer : MonoBehaviour
{
    [SerializeField] UnityEvent m_onHitEnemy = null;

    private void OnTriggerEnter(Collider other) {
        m_onHitEnemy.Invoke();
    }
}
