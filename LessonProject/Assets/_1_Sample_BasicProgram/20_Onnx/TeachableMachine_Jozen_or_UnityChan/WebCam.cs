using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Webカメラ
public class WebCam : MonoBehaviour
{
    // カメラ
    RawImage rawImage;           // RawImage
    WebCamTexture webCamTexture; //Webカメラテクスチャ

    // 推論
    public Classifier classifier;   // 分類
    public TextMeshProUGUI uiText;             // テキスト
    private bool isWorking = false; // 処理中
    WebCamDevice[] devices;         // PCにつないでいるウェブカメラデバイスのリスト
    private string webCamName;

    void Start() {
        
        devices = WebCamTexture.devices;//Webカメラの取得       
        webCamName = devices[2].name;   //PC付属のカメラが[0]のカメラ//USB接続の外付けカメラを指定するなら[1]のカメラ名を取得//作成時、上善のパソコンは1がobsにとられてたので2だった
        // Webカメラの開始
        this.rawImage = GetComponent<RawImage>();
        this.webCamTexture = new WebCamTexture(
            webCamName,
            Classifier.IMAGE_SIZE, Classifier.IMAGE_SIZE, 30);
        this.rawImage.texture = this.webCamTexture;
        this.webCamTexture.Play();
    }

    private void Update() {        
        TFClassify();// 画像分類
    }

    // 画像分類
    private void TFClassify() {
        if (this.isWorking) { return; }
        this.isWorking = true;

        StartCoroutine(ProcessImage(result => {// 画像の前処理
            // 推論の実行
            StartCoroutine(this.classifier.Predict(result, probabilities => {    
                                                        this.uiText.text = "";
                                                        for (int i = 0; i < 2; i++) {
                                                            // 推論結果の表示
                                                            this.uiText.text += probabilities[i].Key + ": " + string.Format("{0:0.000}%", probabilities[i].Value) + "\n";
                                                        }
                                                        Resources.UnloadUnusedAssets();// 未使用のアセットをアンロード
                                                        this.isWorking = false;
                                                        }
                                                   )
                          );
        }));
    }

    // 画像の前処理
    private IEnumerator ProcessImage(System.Action<Color32[]> callback) {
        // 画像のクロップ(切り出し) （WebCamTexture → Texture2D）
        yield return StartCoroutine(CropSquare(webCamTexture, texture =>
                                                {
                                                    // 画像のスケール（Texture2D → Texture2D）
                                                    var scaled = Scaled(texture,Classifier.IMAGE_SIZE,Classifier.IMAGE_SIZE);                                 
                                                    callback(scaled.GetPixels32());// コールバックを返す
                                                }
                                              )
                                   );
    }

    // 画像のクロップ（WebCamTexture → Texture2D）
    public static IEnumerator CropSquare(WebCamTexture texture, System.Action<Texture2D> callback) {
        // Texture2Dの準備
        var smallest = texture.width < texture.height ? texture.width : texture.height;
        var rect = new Rect(0, 0, smallest, smallest);
        Texture2D result = new Texture2D((int)rect.width, (int)rect.height);

        // 画像のクロップ
        if (rect.width != 0 && rect.height != 0) {
            result.SetPixels(
                              texture.GetPixels(
                                                Mathf.FloorToInt((texture.width - rect.width) / 2),
                                                Mathf.FloorToInt((texture.height - rect.height) / 2),
                                                Mathf.FloorToInt(rect.width),
                                                Mathf.FloorToInt(rect.height)
                                                )
                             );
            yield return null;
            result.Apply();
        }

        yield return null;
        callback(result);
    }

    // 画像のスケール（Texture2D → Texture2D）
    public static Texture2D Scaled(Texture2D texture, int width, int height) {       
        var rt = RenderTexture.GetTemporary(width, height); // リサイズ後のRenderTextureの生成
        Graphics.Blit(texture, rt);

        // リサイズ後のTexture2Dの生成
        var preRT = RenderTexture.active;
        RenderTexture.active = rt;
        var ret = new Texture2D(width, height);
        ret.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        ret.Apply();
        RenderTexture.active = preRT;
        RenderTexture.ReleaseTemporary(rt);
        return ret;
    }
    
}
