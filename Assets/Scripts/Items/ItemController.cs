using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemController : MonoBehaviour
{
    [SerializeField] private float timeToDie;
    [SerializeField] private ItemClassEnum _itemClass;
    public ItemClassEnum ItemClass { get { return _itemClass; } set { _itemClass = value; } }
    private Vector3 startingPosition;
    // STATE
    private float restTimeToDie;
    private float restTimeToDiePercent;
    private bool isDying;
    private GameObject player;
    private PlayerExtraTrackers tracker;
    private ItemsManager itemsManager;


    void Awake()
    {
        restTimeToDie = timeToDie;
        isDying = false;
        startingPosition = transform.position;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        itemsManager = GameObject.Find("ItemsManager").GetComponent<ItemsManager>();
        tracker = player.GetComponent<PlayerExtraTrackers>();
    }

    private void Update()
    {
        if (isDying)
        {
            CalculateRestTimeToDie();
            DyingMoveControl();
            DyingFadeControl();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            isDying = true;
            itemsManager.AddCollectedItem(ItemClass);
            tracker.NotifyTracker(itemsManager);
        }
    }

    private void DyingFadeControl()
    {
        if (restTimeToDie <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, restTimeToDiePercent);
        }
    }

    private void DyingMoveControl()
    {
        float interpolatedYPosition = Mathf.Lerp(startingPosition.y+2, startingPosition.y, restTimeToDiePercent);
        
        transform.position = new Vector3(startingPosition.x, interpolatedYPosition, restTimeToDiePercent);
    }
    
    private void CalculateRestTimeToDie()
    {
        restTimeToDie -= Time.deltaTime;
        restTimeToDiePercent = restTimeToDie / timeToDie;
    }
}
