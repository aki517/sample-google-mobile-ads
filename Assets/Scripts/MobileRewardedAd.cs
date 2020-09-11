// リリースビルド時はこれを有効にする.
//#define RELEASE_BUILD

#if !RELEASE_BUILD
    // テスト端末を有効にする.
    #define ENABLE_TEST_DEVICE
    // テスト広告ユニットIDを使用する.
    #define ENABLE_USE_TEST_ADUNIT_ID
#endif //!RELEASE_BUILD

using System;

using UnityEngine;
using UnityEngine.Events;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;


/// <summary>
/// Google Mobile Ads を使ったリワード広告クラス.
/// </summary>
public class MobileRewardedAd : MonoBehaviour
{
    // 広告情報 Android.
    [SerializeField] Info m_info_Android = new Info();
    // 広告情報 iOS.
    [SerializeField] Info m_info_iOS = new Info();

    [Space]

    // MobileAds初期化完了.
    [SerializeField] UnityEvent m_onInitCompleted = null;
    // 広告読込完了.
    [SerializeField] UnityEvent m_onLoadedAd = null;
    // 広告表示時.
    [SerializeField] UnityEvent m_onOpened = null;
    // ユーザが「閉じる」アイコンまたは「戻る」ボタンをタップして広告を閉じる.
    [SerializeField] UnityEvent m_onClosed = null;
    // 広告関連の処理失敗.
    [SerializeField] EventError m_onFailed = null;
    // 動画広告を視聴したユーザへの報酬付与時に呼び出される.
    [SerializeField] EventReward m_onEarnedReward = null;

    // リワード広告オブジェクト.
    RewardedAd m_rewardedAd = null;


    /// <summary>
    /// .
    /// </summary>
    private void Start()
    {
        // 広告呼び出し前に１度呼び出す必要がある.
        MobileAds.Initialize( OnInitCompleted );
    }

    /// <summary>
    /// 広告ユニットIDを取得.
    /// </summary>
    private string GetAdUnitID()
    {
        string adUnitId = "unexpected_platform";

        #if ENABLE_USE_TEST_ADUNIT_ID
            // テストリワード広告ユニットID 設定.
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
            #endif
        #else
            // リリース版のリワード広告ユニットID 設定.
            #if UNITY_ANDROID
                adUnitId = m_info_Android.adUnitId;
            #elif UNITY_IPHONE
                adUnitId = m_info_iOS.adUnitId;
            #endif
        #endif // ENABLE_USE_TEST_ADUNIT_ID

        return adUnitId;
    }

    /// <summary>
    /// リワード広告オブジェクトの作成と読込.
    /// </summary>
    void CreateAndLoadRewardedAd()
    {
        // 広告ユニットIDを指定してリワード広告オブジェクトを作成.
        string adUnitId = GetAdUnitID();
        m_rewardedAd = new RewardedAd( adUnitId );

        // イベントハンドラを設定.
        m_rewardedAd.OnAdLoaded += OnAdLoaded;
        m_rewardedAd.OnAdFailedToLoad += OnAdFailedToLoad;
        m_rewardedAd.OnAdOpening += OnAdOpening;
        m_rewardedAd.OnAdFailedToShow += OnAdFailedToShow;
        m_rewardedAd.OnAdClosed += OnAdClosed;
        m_rewardedAd.OnUserEarnedReward += OnUserEarnedReward;

        // リワード広告を読込.
        AdRequest request = new AdRequest.Builder()
                                #if ENABLE_TEST_DEVICE
                                        .AddTestDevice( AdRequest.TestDeviceSimulator )
                                    #if UNITY_ANDROID
                                        .AddTestDevice( m_info_Android.testDeviceId )
                                    #elif UNITY_IPHONE
                                        .AddTestDevice( m_info_iOS.testDeviceId )
                                    #endif
                                #endif // ENABLE_TEST_DEVICE
                                .Build();

        m_rewardedAd.LoadAd( request );
    }

    /// <summary>
    /// 広告を表示する.
    /// </summary>
    public void Show()
    {
        if( m_rewardedAd.IsLoaded()){
            m_rewardedAd.Show();
        }
    }

    /// <summary>
    /// MobileAds初期化完了時に呼び出される.
    /// </summary>
    private void OnInitCompleted( InitializationStatus initStatus )
    {
        Debug.Log( "Initialization completed." );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onInitCompleted.Invoke(); });

        CreateAndLoadRewardedAd();
    }

    /// <summary>
    /// 広告の読込完了時に呼び出される.
    /// </summary>
    void OnAdLoaded( object sender, EventArgs args )
    {
        Debug.Log( "Loaded RewardedAd." );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onLoadedAd.Invoke(); });
    }
    /// <summary>
    /// 広告の読込失敗時に呼び出される.
    /// </summary>
    void OnAdFailedToLoad( object sender, AdErrorEventArgs errorArgs )
    {
        Debug.LogError( errorArgs.Message );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onFailed.Invoke( errorArgs.Message ); });
    }
    /// <summary>
    /// 広告が表示されると呼び出される.
    /// </summary>
    void OnAdOpening( object sender, EventArgs args )
    {
        // 必要に応じてアプリの音声やゲームループを停止する.
        Debug.Log( "RewardedAd is shown." );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onOpened.Invoke(); });
    }
    /// <summary>
    /// 広告の表示が失敗時に呼び出される.
    /// </summary>
    void OnAdFailedToShow( object sender, AdErrorEventArgs errorArgs )
    {
        Debug.LogError( errorArgs.Message );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onFailed.Invoke( errorArgs.Message ); });
    }
    /// <summary>
    /// ユーザが「閉じる」アイコンまたは「戻る」ボタンをタップして広告を閉じると呼び出される.
    /// </summary>
    void OnAdClosed( object sender, EventArgs args )
    {
        // 必要に応じてアプリの音声やゲームループを再開する.
        Debug.Log( "RewardedAd is closed." );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onClosed.Invoke(); });

        // 次回表示用にRewardedAdオブジェクト作成と広告の読込を行う.
        CreateAndLoadRewardedAd();
    }
    /// <summary>
    /// 動画広告を視聴したユーザへのリワード付与時に呼び出される.
    /// </summary>
    void OnUserEarnedReward( object sender, Reward reward )
    {
        // Google AdMobで作成した広告ユニットの報酬情報.
        Debug.Log( "Earned reward  " + reward.Type + " num " + reward.Amount );
        MobileAdsEventExecutor.ExecuteInUpdate( ()=>{ m_onEarnedReward.Invoke( reward.Type, reward.Amount ); });
    }


    /// <summary>
    /// プラットフォームごとに設定する情報.
    /// </summary>
    [System.Serializable]
    public class Info
    {
        // 広告ユニットID.
        public string adUnitId = string.Empty;
        // テストデバイスID.
        public string testDeviceId = string.Empty;
    }


    [System.Serializable] 
    public class EventError : UnityEvent<string>{}
    [System.Serializable] 
    public class EventReward : UnityEvent<string, double>{}


}   // End of class MobileRewardedAd.
