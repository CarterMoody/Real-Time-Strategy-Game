using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Sprite codeMonkeyHeadSprite;


    public Transform pfProjectileBulletDefault;
    
    public Transform pfDEsoldier;

    public Transform pfEnemy; // this will be set to pfDEsoldier for now

    public Transform pfUSsoldier;

    public Transform pfUnit; // this will be set to pfUSsoldier for now

    public Transform pfUnit_DE;
    public Transform pfUnit_US;




    // his
    public Sprite s_ShootFlash;
    
    public Transform pfSwordSlash;
    public Transform pfEnemyFlyingBody;
    public Transform pfImpactEffect;
    public Transform pfDamagePopup;
    public Transform pfDashEffect;

    public Material m_WeaponTracer;
    public Material m_MarineSpriteSheet;


    public Material m_EnemyYellow;
    public Material m_EnemyOrange;
    public Material m_EnemyRed;


    public Sprite s_USGunner;

    public Sprite s_USSoldier;

    public Sprite s_USPinned;


}
