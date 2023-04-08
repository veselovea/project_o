using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFiller : MonoBehaviour
{
    public GameObject mainBlock;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FillBlocks());
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator FillBlocks()
    {
        float initialX = -149.5f;
        float initialY = 149.5f;

        float originalX = -149.5f;

        int counter = 0;
        for (int i = 0; i < 300; i++)
        {
            initialX = originalX;

            for (int j = 0; j < 300; j++)
            {
                counter++;

                if(counter % 300 == 0)
                {
                    yield return new WaitForSeconds(0.01f);
                }

                Collider2D HitCollider = Physics2D.OverlapCircle(new Vector2(initialX, initialY), 0f);

                if(HitCollider == null)
                {
                    Instantiate(mainBlock, new Vector3(initialX, initialY, 0), Quaternion.identity);
                }

                initialX += 1f;
            }

            initialY -= 1f;
        }
    }
}
