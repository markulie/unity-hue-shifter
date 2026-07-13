using UnityEngine;

[System.Serializable]
public readonly struct HSBColor
{
    public readonly float h;
    public readonly float s;
    public readonly float b;
    public readonly float a;

    public HSBColor(float h, float s, float b, float a = 1f)
    {
        this.h = Mathf.Repeat(h, 1f);
        this.s = Mathf.Clamp01(s);
        this.b = Mathf.Clamp01(b);
        this.a = Mathf.Clamp01(a);
    }

    public HSBColor(Color color)
    {
        HSBColor temp = FromColor(color);
        h = temp.h;
        s = temp.s;
        b = temp.b;
        a = temp.a;
    }

    public static HSBColor FromColor(Color color)
    {
        float r = color.r;
        float g = color.g;
        float b = color.b;

        float max = Mathf.Max(r, Mathf.Max(g, b));
        float min = Mathf.Min(r, Mathf.Min(g, b));

        float delta = max - min;

        float hue = 0f;

        if (delta > Mathf.Epsilon)
        {
            if (Mathf.Approximately(max, r))
            {
                hue = (g - b) / delta;
                if (g < b)
                    hue += 6f;
            }
            else if (Mathf.Approximately(max, g))
            {
                hue = ((b - r) / delta) + 2f;
            }
            else
            {
                hue = ((r - g) / delta) + 4f;
            }

            hue /= 6f;
        }

        float saturation = max <= Mathf.Epsilon ? 0f : delta / max;

        return new HSBColor(hue, saturation, max, color.a);
    }

    public static Color ToColor(HSBColor hsv)
    {
        if (hsv.s <= Mathf.Epsilon)
        {
            return new Color(hsv.b, hsv.b, hsv.b, hsv.a);
        }

        float h = Mathf.Repeat(hsv.h, 1f) * 6f;
        int sector = Mathf.FloorToInt(h);
        float fraction = h - sector;

        float p = hsv.b * (1f - hsv.s);
        float q = hsv.b * (1f - hsv.s * fraction);
        float t = hsv.b * (1f - hsv.s * (1f - fraction));

        return sector switch
        {
            0 => new Color(hsv.b, t, p, hsv.a),
            1 => new Color(q, hsv.b, p, hsv.a),
            2 => new Color(p, hsv.b, t, hsv.a),
            3 => new Color(p, q, hsv.b, hsv.a),
            4 => new Color(t, p, hsv.b, hsv.a),
            _ => new Color(hsv.b, p, q, hsv.a),
        };
    }

    public Color ToColor() => ToColor(this);

    public override string ToString()
    {
        return $"H:{h:F3} S:{s:F3} B:{b:F3} A:{a:F3}";
    }

    public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
    {
        float hue;
        float saturation;

        if (a.b <= Mathf.Epsilon)
        {
            hue = b.h;
            saturation = b.s;
        }
        else if (b.b <= Mathf.Epsilon)
        {
            hue = a.h;
            saturation = a.s;
        }
        else
        {
            if (a.s <= Mathf.Epsilon)
                hue = b.h;
            else if (b.s <= Mathf.Epsilon)
                hue = a.h;
            else
                hue = Mathf.Repeat(Mathf.LerpAngle(a.h * 360f, b.h * 360f, t) / 360f, 1f);

            saturation = Mathf.Lerp(a.s, b.s, t);
        }

        return new HSBColor(
            hue,
            saturation,
            Mathf.Lerp(a.b, b.b, t),
            Mathf.Lerp(a.a, b.a, t));
    }
}