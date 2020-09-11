using UnityEngine;

/// <summary>
/// .
/// </summary>
public class GameEnemy : MonoBehaviour
{
    Vector3 m_initPos;

    void Awake(){
        m_initPos = transform.localPosition;
    }

    void OnEnable(){
        ResetStatus();
    }

    void ResetStatus()
    {
        transform.localPosition = m_initPos;

        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce( Vector3.down * 0.3f, ForceMode.Impulse );
    }

    void Update()
    {
        // プレイヤーより後ろに行ったら初期位置に戻す.
        if( transform.localPosition.z < -10f ){
            ResetStatus();
        }
    }
}
