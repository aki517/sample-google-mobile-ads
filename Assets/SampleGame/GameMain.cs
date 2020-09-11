using UnityEngine;

/// <summary>
/// リワード広告テスト用のサンプルゲームクラス.
/// </summary>
public class GameMain : MonoBehaviour
{
    [SerializeField] GameObject m_uiTitle = null;
    [SerializeField] GameObject m_uiMainLoop = null;
    [SerializeField] GameObject m_uiGameOver = null;

    [SerializeField] Rigidbody m_player = null;
    [SerializeField] Rigidbody[] m_enemies = null;


    void Start()
    {
        // タイトルへ.
        GoToTitle();
    }


    #region TITLE_METHODS
    void GoToTitle()
    {
        m_uiTitle.SetActive( true );
        m_uiMainLoop.SetActive( false );
        m_uiGameOver.SetActive( false );

        m_player.MovePosition( new Vector3( 0, 0, -7.5f ));
        m_player.velocity = Vector3.zero;

        foreach( var enemy in m_enemies ){
            enemy.gameObject.SetActive( false );
        }
    }
    
    public void OnClickButtonStart()
    {
        GoToMain();
    }
    #endregion // TITLE_METHODS


    #region MAIN_METHODS
    void GoToMain()
    {
        Time.timeScale = 1.0f;

        m_uiTitle.SetActive( false );
        m_uiMainLoop.SetActive( true );
        m_uiGameOver.SetActive( false );

        m_player.MovePosition( new Vector3( 0, 0, -7.5f ));
        m_player.velocity = Vector3.zero;

        foreach( var enemy in m_enemies ){
            enemy.gameObject.SetActive( false );
            enemy.gameObject.SetActive( true );
        }
    }

    public void OnClickButtonMoveR()
    { 
        m_player.velocity = Vector3.right * 2f; 
    }

    public void OnClickButtonMoveL()
    {
        m_player.velocity = Vector3.left * 2f;
    }

    public void OnHitEnemy()
    {
        GoToGameOver();
    }
    #endregion // MAIN_METHODS


    #region GAME_OVER_METHODS
    void GoToGameOver()
    {
        m_uiTitle.SetActive( false );
        m_uiMainLoop.SetActive( false );
        m_uiGameOver.SetActive( true );

        Time.timeScale = 0.0f;
    }
    public void OnClickButtonBackToTitle()
    {
        GoToTitle();
    }
    #endregion // GAME_OVER_METHODS


    #region MOBILE_ADS_METHODS
    public void OnAdInitCompleted()
    {
    }

    public void OnAdLoaded()
    {
    }

    public void OnAdOpened()
    {
    }

    public void OnAdClosed()
    {
    }

    public void OnAdEarnedReward( string rewardType, double rewardAmount )
    {
        // ゲームへ復帰.
        GoToMain();
    }

    public void OnAdFiled( string errorMessage )
    {
        // 失敗したらタイトルへ.
        GoToTitle();
    }
    #endregion // MOBILE_ADS_METHODS







}
