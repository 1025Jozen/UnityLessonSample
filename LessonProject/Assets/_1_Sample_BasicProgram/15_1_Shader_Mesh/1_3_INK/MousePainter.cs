using UnityEngine;

//マウスで色づけるためのクラス
public class MousePainter : MonoBehaviour
{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;
    
    public float radius   = 1;
    public float strength = 1;
    public float hardness = 1;

    void Update(){

        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);//マウスクリックを連続的に取得するか単発で取得するかどうか

        if (click){
            Vector3 position = Input.mousePosition;  //画面上のマウスの位置取得
            Ray ray = cam.ScreenPointToRay(position);//カメラからレイを飛ばす
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f)){
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;//当たった位置に
                Paintable p        = hit.collider.GetComponent<Paintable>(); //とりあえずPaintableをつけてるかどうか判断
                if (p != null) {
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                    //Paintableがついてたら
                    //その場所に　p Paintableクラスのアドレス情報・hit.point当たった場所情報・半径・hardness・strength・paintColor情報を渡してお絵描き
                }
            }
        }

    }

}
