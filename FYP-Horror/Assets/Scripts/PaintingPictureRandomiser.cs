using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingPictureRandomiser : MonoBehaviour
{
    public MeshRenderer renderer;
    public List<Sprite> sprites;

    public int minScale;
    public int maxScale;
    // Start is called before the first frame update
    void Start()
    {
        int randomSprite = Random.Range(0, sprites.Count);

        int randomScale = Random.Range(minScale, maxScale);

        this.gameObject.transform.localScale = this.gameObject.transform.localScale * (randomScale / 100);
        renderer.materials[1].mainTexture = sprites[randomSprite].texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
