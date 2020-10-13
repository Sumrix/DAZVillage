using UnityEngine;
using System.Collections;

// Ядро объекта сцены
public class CoreObject :
    MonoBehaviour
{
    public string ID;
    protected bool _isInited = false;
    [SerializeField]
    public Sprite Sprite;
    [SerializeField]
    public GameObject Model;
    
    protected virtual void Start()
    {
        // Инстантим модельку
        if (!_isInited)
        {
            InitModel();
            _isInited = true;
        }
        // Для срабатывания OnTriggerEnter
        var collider = GetComponent<Collider>();
        if (collider != null && collider.enabled)
        {
            collider.enabled = false;
            collider.enabled = true;
        }
    }
    protected virtual void InitModel()
    {
        if (Model != null)
        {
            Model = (GameObject)Instantiate(Model, transform, false);
        }
    }
    public void Destroy()
    {
        if (isActiveAndEnabled)
        {
            StartCoroutine(DestroyImpl());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Для срабатывания OnTriggerExit
    private IEnumerator DestroyImpl()
    {
        transform.parent = null;
        // Удачного падения в бесконечность!
        transform.position = new Vector3(-1000, -1000, -1000);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
