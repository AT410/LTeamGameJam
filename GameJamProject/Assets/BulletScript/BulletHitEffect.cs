using System.Collections;
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    //エフェクトのプレハブへのパス
    private const string EFFECT_PATH = "Effect/Collision";

    void OnCollisionEnter2D(Collision2D collider)
    {
        //衝突エフェクト
        foreach (ContactPoint2D point in collider.contacts)
        {
            GameObject effect = Instantiate(Resources.Load(EFFECT_PATH)) as GameObject;
            effect.transform.position = (Vector3)point.point;
        }

    }
}
