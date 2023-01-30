using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static BattleManager;

/// <summary>
/// 后续使用对象池
/// 
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance { get; private set; }

    public List<ZombieController> zombieList = new List<ZombieController>();

    public GameObject zombiePrefab;

    public float zombieCylinderHeight;

    public Transform zombieRoot;


    public int zombiePrewarm = 100;
    public ZombiePool zbPool;

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

        zombieCylinderHeight = zombiePrefab.GetComponent<ZombieController>().CylinderHeight;

        zbPool = new ZombiePool();
        zbPool.InitParam(zombiePrefab, zombieRoot, 20);
        zbPool.InitSpawn();
    }

    public interface IPool<T>
    {
        void InitParam(GameObject prefab, Transform root, int prewarm = 100);
        void InitSpawn();
        T Get();
        void Return(T t);
    }

    public class ZombiePool : IPool<ZombieController>
    {
        public int prewarm = 100;

        private Stack<ZombieController> pool;

        private GameObject prefab;
        private Transform root;

        public void InitParam(GameObject prefab, Transform root, int prewarm = 100)
        {
            this.prefab = prefab;
            this.root = root;
            this.prewarm = prewarm;
        }

        public void InitSpawn()
        {
            pool = new Stack<ZombieController>();
            for (int i = 0; i < this.prewarm; i++)
            {
                GameObject go = Instantiate(prefab, root);
                var controller = go.GetComponent<ZombieController>();

                controller.SetStatus_ARMandAUP(false);

                pool.Push(controller);
                go.SetActive(false);
            }
        }

        public ZombieController Get()
        {
            if(!pool.TryPop(out var controller))
            {
                // 池子里 没有余量
                prewarm += 50;
                AppendSpawn(50);
                controller = pool.Pop();
            }
            controller.gameObject.SetActive(true);

            var pos = GetSpawnPos();

            controller.gameObject.transform.localPosition = pos;
            //Debug.Log(controller.gameObject.transform.localPosition);

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.localPosition = pos;
            //go.transform.localScale = Vector3.one;
            //go.GetComponent<SphereCollider>().enabled = false;

            controller.MaxHealth = 30f;
            controller.CurHealth = 30f;

            controller.StartCoroutine(controller.SS(true));

            return controller;
        }



        public void Return(ZombieController item)
        {
            pool.Push(item);
            item.gameObject.SetActive(false);
        }

        private void AppendSpawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(prefab, root);
                var controller = go.GetComponent<ZombieController>();
                controller.SetStatus_ARMandAUP(false);
                pool.Push(go.GetComponent<ZombieController>());
                go.SetActive(false);
            }
        }

        public Vector3 GetSpawnPos()
        {
            PlayerController player = PlayerController.instance;

            float randomDist = UnityEngine.Random.Range(player.spawnEnemyInnerRadius, player.spawnEnemyOutterRadius);
            float randomAngle = UnityEngine.Random.Range(0f, 2f);
            float Xoffset = Mathf.Cos(randomAngle * Mathf.PI) * randomDist;
            float Zoffset = Mathf.Sin(randomAngle * Mathf.PI) * randomDist;
            Vector3 xzPos = new Vector3(Xoffset + player.transform.position.x, 1000f, Zoffset + player.transform.position.z);

            // 射线检测得到Y轴坐标
            LayerMask mask = 1 << 7;
            Physics.Raycast(xzPos, Vector3.down, out var hitInfo, 2000f, mask);
            Vector3 pos = new Vector3(xzPos.x, hitInfo.point.y + BattleManager.instance.zombieCylinderHeight / 2 + 0.1f, xzPos.z);

            // 如果生在树上 重新射 直到生在地上 返回地上的正确位置
            if (hitInfo.collider.gameObject.CompareTag("Tree"))
            {
                pos = GetSpawnPos();
            }
            // 同样 不能出生在 Navemesh不可移动的位置上 返回false说明采样失败 我们就重新生成位置
            bool isInCanMoveArea = NavMesh.SamplePosition(pos, out var hit, 2f, 1);
            if (!isInCanMoveArea)
            {
                pos = GetSpawnPos();
            }
            return pos;
        }
    }



    private void Start()
    {

    }

    private void Update()
    {
        int wholePart = Mathf.FloorToInt(Time.time);
        int fractPart = Mathf.FloorToInt((Time.time - wholePart) * 100);

        int minutesNum = wholePart / 60;
        wholePart %= 60;

        // 前两分钟
        if (minutesNum < 2)
        {
            if (Time.frameCount % (60 * 2) == 0)
            {
                //GameObject zombie = Instantiate(zombiePrefab, GetSpawnPos(), Quaternion.identity, zombieRoot);
                //Debug.Log("生成了一个");

                var item = zbPool.Get();
                zombieList.Add(item);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"timescale = {Time.timeScale}");
            Time.timeScale = 2f;
        }
    }



    private void OnGUI()
    {
        GUIStyle style_32_bold_rich = new GUIStyle() { fontSize = 32, fontStyle = FontStyle.Bold, richText = true };

        int wholePart = Mathf.FloorToInt(Time.time);
        int fractPart = Mathf.FloorToInt((Time.time - wholePart) * 100);

        int minutesNum = wholePart / 60;
        wholePart %= 60;

        string secNum = wholePart < 10 ? $"0{wholePart}" : $"{wholePart}";
        string minNum = minutesNum < 10 ? $"0{minutesNum}" : $"{minutesNum}";

        GUI.Label(new Rect(Screen.width / 2.0f, 60, 100, 100), $"{minNum}:{secNum}:{fractPart}\nDPI:{Screen.dpi}", style_32_bold_rich);
    }


}
