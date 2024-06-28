using UnityEngine;
using UnityEngine.Serialization;

public class Writeable : MonoBehaviour
{
    public Vector2 textureSize = new Vector2(1024, 1024);
    public Texture2D texture;

    [SerializeField] private Renderer _renderer;

    private void Start()
    {
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        _renderer.material.mainTexture = texture;
    }
}
