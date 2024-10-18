using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] pilesOfDirt;
    float pilesAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnPileOfDirt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnPileOfDirt() {
        for (int i = 0; i < 10; i++) 
        {
            if(i==0 || i==1 || i==2 || i==3 || i==4)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-20.0f, 14.0f), -0.3f, Random.Range(-25.0f, 18.0f));
                Instantiate(pilesOfDirt[i], spawnPosition, Quaternion.identity);
                pilesAmount ++;
                yield return new WaitForSeconds(0.1f);
            } 
            else 
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-20.0f, 14.0f), -0.3f, Random.Range(-25.0f, 18.0f));
                Instantiate(pilesOfDirt[5], spawnPosition, Quaternion.identity);
                pilesAmount ++;
                yield return new WaitForSeconds(0.1f);
            }   
        }
    }
}
