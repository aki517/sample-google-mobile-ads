# sample-google-mobile-ads
Google Mobile Ads Unity Plugin の検証用プロジェクト  

![demo](https://github.com/aki517/search-filter-imp-exp/wiki/img/admob002.gif)  

### Unity Version
Unity 2019.4.3f1

### MobileRewardedAd
RewardedAdオブジェクトを使ったリワード広告制御を行うスクリプト  

#### Inspectorビュー
以下の設定が行なえます。  

・プラットフォーム毎の広告ユニットIDとテストデバイスIDの設定  
![demo](https://github.com/aki517/search-filter-imp-exp/wiki/img/admob001.png)  

・リワード広告用イベントの設定  
|イベント|内容|
|-|-|
|OnInitCompleted| MobileAds初期化完了時に呼び出される|
|OnAdLoaded| 広告の読込完了時に呼び出される|
|OnAdFailedToLoad| 広告の読込失敗時に呼び出される|
|OnAdOpening| 広告が表示されると呼び出される|
|OnAdFailedToShow| 広告の表示が失敗時に呼び出される|
|OnAdClosed| ユーザが「閉じる」アイコンまたは「戻る」ボタンをタップして広告を閉じると呼び出される|
|OnUserEarnedReward| 動画広告を視聴したユーザへのリワード付与時に呼び出される|

#### その他
RELEASE_BUILD を有効にするとスクリプトからテストに関するコードを除外します。  

### SampleGame
リワード広告の確認用サンプルゲームが始まります。  
球に当たるとゲームオーバーとなりますがリワード広告を視聴することで  
ゲームを継続することができる仕様です。  

なるべくスクリプト書きたくなかったのでUnityEventを使って  
Inspectorから衝突、操作などのイベントを設定しています。  