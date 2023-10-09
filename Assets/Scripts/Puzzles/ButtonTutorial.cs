using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Reference
public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] private string keyToPress;     // Key displayed within brackets in the prompt;
	[SerializeField] private GameObject arrow;      // Reference to arrow prefab;
    [SerializeField] private GameObject parent;     // Reference to parent;

    private GameObject _myArrow;        // Reference to arrow spawned;
    private SpriteRenderer _arrowSpr;   // Reference to the arrow sprite;
    
    private float _arrowSpace = 1.25f;  ////
    private float _topPoint;            //
    private float _botPoint;            //
    private float _spdDown = 1f;        // Variables to control the motion of the arrow;
	private float _spdUp = 0.75f;       //
    private float _arrowSpeed = 1f;     //
	private bool _arrowUp = false;      ////

    private SpriteRenderer _parentSpr;  // Reference to super sprite renderer;
    private TextMeshPro _textComp;      // Reference to text mesh;
    private byte _r = 255, _g = 255, _b = 255, _alpha = 0;    // RGBA values;
    private bool _fade = false;         // Whether the pop-up is fading or not;

    // Initialize variables and instances, set-up scale, positions, rotations, etc.
    void Start() {
        transform.localScale = Vector2.one;
		_textComp = GetComponent<TextMeshPro>();
        _textComp.text = ("Press [" + keyToPress.ToUpper() + "]\nTo Interact").Replace("\\n", "\n");
        _textComp.color = new Color32(_r, _g, _b, _alpha);
        if (parent != null && parent.GetComponent<SpriteRenderer>() != null) {
            _parentSpr = parent.GetComponent<SpriteRenderer>();
		} else {
            Debug.Log("Button Tutorial unnassigned or has Sprite-less Parent");
        }
		transform.position = new Vector3(transform.position.x,
										 transform.position.y + _parentSpr.bounds.size.y/2 + _arrowSpace,
										 transform.position.z);
        transform.rotation = Quaternion.identity;
        _myArrow = Instantiate(arrow, new Vector3(transform.position.x,
                                                  transform.position.y - GetComponent<RectTransform>().rect.height*3/4 + 0.05f,
                                                  transform.position.z), Quaternion.identity);
        _topPoint = _myArrow.transform.position.y;
        _botPoint = parent.transform.position.y + _parentSpr.bounds.size.y/2 + 0.3f;
        _arrowSpr = _myArrow.GetComponent<SpriteRenderer>();
		_textComp.color = new Color32(_r, _g, _b, 0);
		_arrowSpr.color = new Color32(255, 255, 255, 0);
	}

    // Controls fading and the movement of the arrow;
    void Update() {
        if (_alpha <= 245 && !_fade) {
            _alpha += 10;
			_textComp.color = new Color32(_r, _g, _b, _alpha);
            _arrowSpr.color = new Color32(255, 255, 255, _alpha);
		}
        if (_myArrow != null) {
            if (_arrowUp) {
                if (_myArrow.transform.position.y <= _topPoint) {
                    _myArrow.transform.Translate(Vector3.up * _arrowSpeed * Time.deltaTime);
                }
                else {
                    _arrowUp = false;
                    _arrowSpeed = _spdDown;
                }
            }
            else {
                if (_myArrow.transform.position.y >= _botPoint) {
                    _myArrow.transform.Translate(Vector3.down * _arrowSpeed * Time.deltaTime);
                }
                else {
                    _arrowUp = true;
                    _arrowSpeed = _spdUp;
                }
            }
        }
        if (_fade) {
			_alpha -= 10;
			_textComp.color = new Color32(_r, _g, _b, _alpha);
			_arrowSpr.color = new Color32(255, 255, 255, _alpha);
            if (_alpha <= 10) {
                Destroy(_myArrow);
                Destroy(gameObject);
            }
		}
    }

    // Method to set up the button tutorial
    public void SetUp(string keyToPress, GameObject parent) {
        this.keyToPress = keyToPress;
        this.parent = parent;
    }

    // Methods called by the parent object to control fading;
    // Hello CS 1331 ;-;
    public void Fade() {
        _fade = true;
    }
    public void CancelFade() {
        _fade = false;
    }
}