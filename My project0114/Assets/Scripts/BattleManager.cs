using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 后续使用对象池
/// 
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }

    public List<ZombieController> zombieList = new List<ZombieController>();


    public void Destroy()
    {
        if (instance != null)
        {
            foreach (var item in zombieList)
            {
                item.Destroy();
            }
            zombieList = null;
            instance = null;
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        // 单例
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


}
