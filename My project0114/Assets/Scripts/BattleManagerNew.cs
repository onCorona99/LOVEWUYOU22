using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleManagerNew : MonoBehaviour
{
    public static BattleManagerNew instance { get; private set; }

    public float spawnInterval = 4f;

    public List<ZombieController> zombieList = new List<ZombieController>();

    public GameObject zombiePrefab;

    public Transform zombieRoot;

    public bool autoGenerate = false;

    private float zombieCylinderHeight;

    private void Awake()
    {
        // ����
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        zombieCylinderHeight = zombiePrefab.GetComponent<ZombieController>().CylinderHeight;
   
    }

 
    void Update()
    {
        int wholePart = Mathf.FloorToInt(Time.time);
        int fractPart = Mathf.FloorToInt((Time.time - wholePart) * 100);

        int minutesNum = wholePart / 60;
        wholePart %= 60;

        // ǰ������
        if (autoGenerate)
        {
            if (minutesNum < 2)
            {
                if (Time.frameCount % (60 * spawnInterval) == 0)
                {
                    GameObject zombie = Instantiate(zombiePrefab, GetSpawnPos(), Quaternion.identity, zombieRoot);
                    Debug.Log("������һ��");
                    zombieList.Add(zombie.GetComponent<ZombieController>());
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GameObject zombie = Instantiate(zombiePrefab, GetSpawnPos(), Quaternion.identity, zombieRoot);
                Debug.Log("������һ��");
                zombieList.Add(zombie.GetComponent<ZombieController>());
            }
        }
       

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"timescale = {Time.timeScale}");
            Time.timeScale = 2f;
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

        // ���߼��õ�Y������
        LayerMask mask = 1 << 7;
        Physics.Raycast(xzPos, Vector3.down, out var hitInfo, 2000f, mask);
        Vector3 pos = new Vector3(xzPos.x, hitInfo.point.y + BattleManagerNew.instance.zombieCylinderHeight / 2 + 0.1f, xzPos.z);

        // ����������� ������ ֱ�����ڵ��� ���ص��ϵ���ȷλ��
        if (hitInfo.collider.gameObject.CompareTag("Tree"))
        {
            pos = GetSpawnPos();
        }
        // ͬ�� ���ܳ����� Navemesh�����ƶ���λ���� ����false˵������ʧ�� ���Ǿ���������λ��
        bool isInCanMoveArea = NavMesh.SamplePosition(pos, out var hit, 2f, 1);
        if (!isInCanMoveArea)
        {
            pos = GetSpawnPos();
        }
        return pos;
    }

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
