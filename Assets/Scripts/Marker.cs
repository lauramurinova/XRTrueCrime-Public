using System.Linq;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;
    private Writeable _writeable;
    private bool _touchedLastFrame;

    private void Start()
    {
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        _tipHeight = _tip.localScale.y/10;
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Writeable"))
            {
                if (_writeable == null)
                {
                    _writeable = _touch.transform.GetComponent<Writeable>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _writeable.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _writeable.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _writeable.textureSize.y || x < 0 || x > _writeable.textureSize.x)
                {
                    Debug.Log(y);
                    Debug.Log(_writeable.textureSize.y);
                    Debug.Log(x);
                    return;
                }
                Debug.Log("SOM TU");
                if (_touchedLastFrame)
                {
                    Debug.Log("SOM TU");
                    _writeable.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _writeable.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }

                    transform.rotation = _lastTouchRot;

                    _writeable.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }
        _writeable = null;
        _touchedLastFrame = false;
    }
}
