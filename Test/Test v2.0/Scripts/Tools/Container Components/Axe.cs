using UnityEngine;

public class Axe : Tools
{
    public override NamesOfTools ToolsName { get; protected set; } = NamesOfTools.Axe;
    public override Collider2D ToolsColliider { get; protected set; }
    public override int Damage { get; protected set; } = 50;
    public override float Speed { get; protected set; } = 0.5f;

    public void Start()
    {
        ToolsColliider = GetComponent<Collider2D>();
    }
}
