using UnityEngine;

public class DownSampleAnimation : MonoBehaviour
{
    [SerializeField] private DownSamplingRenderFeature _feature;// Featureの参照を保持  (FeatureのパラメーターからPassにSetParam)

    void Update()
    {
        //
        var tmp = (Mathf.Sin(4f * Time.frameCount * Mathf.PI / 180f) + 1f) / 2f;
        var value = tmp * 120f;          // ダウンサンプリング最小1/120のサイズまで適用 
        _feature.downSample = (int)value;// Featureのパラメータを更新 
    }
}