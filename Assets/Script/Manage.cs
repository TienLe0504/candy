using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manage : MonoBehaviour
{
    public static Manage instance;
    public int width;
    public int height;
    [SerializeField] public GameObject tilePrefab;
    public GameObject[] dots;
    public Board board;
    public FindMatches findMatches;
    public GameObject explode;
    public Dot currentDot;

    [SerializeField] public GameObject explodeHorizontal;
    [SerializeField] public GameObject explodeVertical;
    [SerializeField] public GameObject explodeColor;
    [SerializeField] public int amountToCreateBombColor;
    public int amountCurrent=0;

    [Header("Sware")]
    [SerializeField] public List<SwareSpecial> dotCold;
    [SerializeField] public List<SwareSnow> dotSnow;
    //[SerializeField] public GameObject swareCold;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
