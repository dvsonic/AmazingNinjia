using UnityEngine;
using System.Collections;

public class BlockFactory : MonoBehaviour {

	// Use this for initialization
    public GameObject[] blockList;
    public GameObject specialBlock;
    public GameObject[] bananaList;
    public float minYOffset = 0;
    public float maxYOffset = 1;
    public float minXOffset = -0.5f;
    public float maxXOffset = 0.5f;
    public Transform lastBlock;
    public GameObject[] obstacleList;
    private static BlockFactory _instant;
	void Start () {
        _instant = this;
	}

    public static BlockFactory getInstance()
    {
        return _instant;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
    }


    private int blockNum;
    public void CreatBlock()
    {
        blockNum++;
        
        Object prefab;
        if (blockNum % 12 < 4 && blockNum>10)
            prefab = specialBlock;
        else
            prefab = blockList[Random.Range(0, blockList.Length)];

        GameObject obj = Instantiate(prefab) as GameObject;

        float offsetX = Random.Range(minXOffset,maxXOffset);
        float offsetY = Random.Range(minYOffset,maxYOffset);
        float oldWidth = GetBlockWidth(lastBlock.gameObject);
        float newWidth = GetBlockWidth(obj);
        obj.transform.position = new Vector3(lastBlock.transform.position.x + oldWidth + offsetX, offsetY);
        lastBlock = obj.transform;

        float percent = 0.5f;//概率
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            if (child.gameObject.tag == "SpawnPoint")
            {
                if (Random.Range(0.0f, 1.0f) < percent)//概率召唤单位
                {
                    percent -= 0.1f;
                    Object prefab2 = obstacleList[Random.Range(0, obstacleList.Length)];
                    Instantiate(prefab2, child.position, child.rotation);
                }
                else
                {
                    percent += 0.1f;
                }
            }
        }

        if (prefab == specialBlock)
            CreatBlock();
    }

    private float GetBlockWidth(GameObject block)
    {
        return block.GetComponent<BoxCollider2D>().size.x;
    }

}
