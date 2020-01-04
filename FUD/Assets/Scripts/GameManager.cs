using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance = null;

    private GameManager()
    {

    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    #endregion

    // Start is called before the first frame update

    [HideInInspector]
    public APIHandler apiHandler;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        apiHandler = new APIHandler();
    }

}
