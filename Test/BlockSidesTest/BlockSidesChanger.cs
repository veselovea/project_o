using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSidesChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        tR = transform.Find("TR").GetComponent<SpriteRenderer>();
        tL = transform.Find("TL").GetComponent<SpriteRenderer>();
        bR = transform.Find("BR").GetComponent<SpriteRenderer>();
        bL = transform.Find("BL").GetComponent<SpriteRenderer>();

        StartCoroutine(DelayedSetSides());
    }

    IEnumerator DelayedSetSides()
    {
        CheckNeighbors();
        yield return new WaitForSeconds(1f);
        CheckSides();
    }

    public SpriteRenderer tR;
    public SpriteRenderer tL;
    public SpriteRenderer bR;
    public SpriteRenderer bL;

    public List<Sprite> sides;

    private string tR4 = "";
    private string bR4 = "";
    private string bL4 = "";
    private string tL4 = "";
    private string tR8 = "";
    private string bR8 = "";
    private string bL8 = "";
    private string tL8 = "";

    public void CheckSides()
    {
        Collider2D top = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(0, 1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D topRight = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, 1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D right = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, 0)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D rightBottom = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D bottom = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(0, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D bottomLeft = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D left = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, 0)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D leftTop = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, 1)), 0f, LayerMask.GetMask("SolidBlock"));

        tR4 = "";
        bR4 = "";
        bL4 = "";
        tL4 = "";
        tR8 = "";
        bR8 = "";
        bL8 = "";
        tL8 = "";

        if (top == null)
        {
            tR4 += "TE";
            tL4 += "TE";

            tR8 += "TE";
            tL8 += "TE";
        }
        else
        {
            tR4 += "TF";
            tL4 += "TF";

            tR8 += "TF";
            tL8 += "TF";
        }

        if (topRight == null)
        {
            tR8 += "TRE";
        }
        else
        {
            tR8 += "TRF";
        }

        if (right == null)
        {
            tR4 += "RE";
            bR4 += "RE";

            tR8 += "RE";
            bR8 += "RE";
        }
        else
        {
            tR4 += "RF";
            bR4 += "RF";

            tR8 += "RF";
            bR8 += "RF";
        }

        if(rightBottom == null)
        {
            bR8 += "BRE";
        }
        else
        {
            bR8 += "BRF";
        }

        if(bottom == null)
        {
            bR4 += "BE";
            bL4 += "BE";

            bR8 += "BE";
            bL8 += "BE";
        }
        else
        {
            bR4 += "BF";
            bL4 += "BF";

            bR8 += "BF";
            bL8 += "BF";
        }

        if (bottomLeft == null)
        {
            bL8 += "BLE";
        }
        else
        {
            bL8 += "BLF";
        }

        if(left == null)
        {
            bL4 += "LE";
            tL4 += "LE";

            bL8 += "LE";
            tL8 += "LE";
        }
        else
        {
            bL4 += "LF";
            tL4 += "LF";

            bL8 += "LF";
            tL8 += "LF";
        }

        if (leftTop == null)
        {
            tL8 += "TLE";
        }
        else
        {
            tL8 += "TLF";
        }

        StartCoroutine(DelayedApplyingSides());
    }

    IEnumerator DelayedApplyingSides()
    {
        yield return new WaitForSeconds(0.1f);
        ApplySides();
        StartCoroutine(CooldownOnApplying());
    }

    bool applyCD = false;

    IEnumerator CooldownOnApplying()
    {
        applyCD = true;
        yield return new WaitForSeconds(1f);
        applyCD = false;
    }

    public void CheckNeighbors()
    {
        Collider2D top = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(0, 1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D topRight = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, 1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D right = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, 0)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D rightBottom = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(1, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D bottom = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(0, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D bottomLeft = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, -1)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D left = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, 0)), 0f, LayerMask.GetMask("SolidBlock"));
        Collider2D leftTop = Physics2D.OverlapCircle(transform.TransformPoint(new Vector2(-1, 1)), 0f, LayerMask.GetMask("SolidBlock"));

        if(top != null)
        {
            top.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (topRight != null)
        {
            topRight.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (right != null)
        {
            right.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (rightBottom != null)
        {
            rightBottom.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (bottom != null)
        {
            bottom.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (bottomLeft != null)
        {
            bottomLeft.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (left != null)
        {
            left.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        if (leftTop != null)
        {
            leftTop.GetComponentInParent<BlockSidesChanger>().CheckSides();
        }

        //if(top == null && topRight == null && right == null && rightBottom == null && bottom == null && bottomLeft == null && left == null && leftTop == null)
        //{
        //    CheckSides();
        //    ApplySides();
        //}
    }

    public void ApplySides()
    {
        Sprite sideTR = sides.Find(s => s.name == $"Stone({tR8})");
        if(sideTR == null)
        {
            sideTR = sides.Find(s => s.name == $"Stone({tR4})");
            if(sideTR == null)
            {
                sideTR = sides[0];
            }
        }

        Sprite sideTL = sides.Find(s => s.name == $"Stone({tL8})");
        if (sideTL == null)
        {
            sideTL = sides.Find(s => s.name == $"Stone({tL4})");
            if (sideTL == null)
            {
                sideTL = sides[0];
            }
        }

        Sprite sideBR = sides.Find(s => s.name == $"Stone({bR8})");
        if (sideBR == null)
        {
            sideBR = sides.Find(s => s.name == $"Stone({bR4})");
            if (sideBR == null)
            {
                sideBR = sides[0];
            }
        }

        Sprite sideBL = sides.Find(s => s.name == $"Stone({bL8})");
        if (sideBL == null)
        {
            sideBL = sides.Find(s => s.name == $"Stone({bL4})");
            if (sideBL == null)
            {
                sideBL = sides[0];
            }
        }

        tR.sprite = sideTR;
        tL.sprite = sideTL;
        bR.sprite = sideBR;
        bL.sprite = sideBL;
    }
}
