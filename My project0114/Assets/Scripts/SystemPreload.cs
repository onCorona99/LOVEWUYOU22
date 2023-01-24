using UnityEngine;

public class SystemPreload : MonoBehaviour
{
    public static SystemPreload instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneSystem.GetInstance().SetScene(new StartScene());
    }
}