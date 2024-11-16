using System.Collections.Generic;
using UnityEngine;

public class ParallaxStars : MonoBehaviour
{
    [SerializeField] int[] starsPerLayer;
    [SerializeField] float[] layerSpeed;
    [SerializeField] Vector2 halfSize;
    [SerializeField] GameObject starPrefab;
    Transform player;
    Vector3 playerDeltaPos;
    Vector3 prevPlayerPos;
    [System.Serializable]
    struct Stars
    {
        public Sprite[] sprites;
    }

    [SerializeField] Stars[] starStruct;

    List<Transform> layers = new List<Transform>();

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        int n = 0;
        foreach (int layer in starsPerLayer)
        {
            Transform newLayer = Instantiate(new GameObject(), transform).transform;
            newLayer.gameObject.name = "layer" + n;
            layers.Add(newLayer);
            for (int i = 0; i < layer; i++)
            {
                GameObject star = Instantiate(starPrefab, layers[n]);
                star.GetComponent<SpriteRenderer>().sprite = starStruct[n].sprites[Random.Range(0, starStruct[n].sprites.Length)];
                star.transform.position = new Vector2(Random.Range(-halfSize.x, halfSize.x), Random.Range(-halfSize.y, halfSize.y));
            }
            n++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDeltaPos = player.position - prevPlayerPos;
        prevPlayerPos = player.position;
        int n = 0;
        foreach (Transform layer in layers)
        {
            layer.transform.position -= playerDeltaPos * layerSpeed[n];
            n++;
        }
    }
}
