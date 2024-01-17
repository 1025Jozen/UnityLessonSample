using UnityEngine;

namespace CustomEditorSamp
{

    [CreateAssetMenu(fileName = "Test", menuName = "MySettings")]
    public class SampleRoot : ScriptableObject
    {
        public string labelText = "Hello, World!";

        //[Range(0, 5)] public int b = 5;
        public string enemyName;
        public int hp;
        //public int power;

        //public static Vector3 vec3;
        ////-------------------------------------------------------
        ////0001 0010 0100
        //public enum Animal { dog = 1 << 0, cat = 1 << 1, bear = 1 << 2 }
        //public static Animal animalKind;


    }
}

//[ColorUsage(bool showAlpha)]
//[ColorUsage(bool showAlpha, bool hdr)] 色を選択する時に、どのやつを使うか。



//showAlpha:アルファ値を使うか。
//hdr:HDRカラーを使うか。
//[Delayed] 数値・文字列を変更する時に Enter が押されるかフォーカスが外れるまで変更しない。
//[GradientUsage(bool hdr)] Gradient を編集する際、HDRカラーを使うか。



//hdr:HDRカラーを使うか。
//[Header(string header)] インスペクターで変数の上にヘッダーをつける。



//header: ヘッダーで表示する内容。
//[HideInInspector] インスペクターに表示しない。
//[InspectorName(string displayName)] インスペクターに表示される名前を設定。



//displayName: インスペクターに表示される内容。
//[Min(float min)] インスペクターで操作できる最小値を設定。



//min: 最小値。
//[Multiline] 文字列を複数行入力できるようにする。
//[Range(float min, float max)] インスペクターで操作できる範囲を設定。



//min: 最小値。
//max: 最大値。
//[SerializeField] シリアライズできるようにする。おおざっぱに書くと、private をインスペクターに表示させるときに使う。
//[Space(float height)] スペースを開ける。



//height: スペースの高さ。px 単位。
//[TextArea]
//[TextArea(int minLines, int maxLines)] 複数行入力できるようにする。



//minLines: インスペクターで確保する最低の行数。
//maxLines: インスペクターで確保する最大の行数。
//[Tooltip(string tooltip)] ツールチップを表示する。



//tooltip: ツールチップで表示する内容。