using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HueShifter : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    [SerializeField]
    private float speed = 1f;

    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        HSBColor color = new HSBColor(
            Mathf.PingPong(Time.time * speed, 1f),
            1f,
            1f);

        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(ColorId, color.ToColor());
        rend.SetPropertyBlock(propertyBlock);
    }
}