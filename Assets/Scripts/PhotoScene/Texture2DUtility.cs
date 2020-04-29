using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Texture2DUtility
{
    public static Texture2D AddWatermark(Texture2D background, Texture2D watermark, int startX, int startY)
    {
        Texture2D newTex = new Texture2D(background.width, background.height, background.format, false);
        for (int x = 0; x < background.width; x++)
        {
            for (int y = 0; y < background.height; y++)
            {
                if (x >= startX && y >= startY && x < watermark.width && y < watermark.height)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    newTex.SetPixel(x, y, final_color);
                }
                else
                    newTex.SetPixel(x, y, background.GetPixel(x, y));
            }
        }

        newTex.Apply();
        return newTex;
    }

    public static void RescaleTexture(Texture2D texture, float rescaleFactor, int sizeOne, int sizeTwo, bool isHeight = false)
    {
        float coeff = sizeOne / rescaleFactor;
        float x = sizeOne / coeff;
        float y = sizeTwo / coeff;
        
        if (isHeight)
        {
            TextureScale.Point(texture, (int)y, (int)x);
        }
        else
        {
            TextureScale.Point(texture, (int)x, (int)y);
        }
    }

    public static void RescaleTextureByHeight(Texture2D texture, float height)
    {
        float coeff = texture.height / height;
        float newHeight = texture.height / coeff;
        float newWindth = texture.width / coeff;

        TextureScale.Point(texture, (int)newWindth, (int)newHeight);
    }

    public static void SplitTextures(Texture2D circleTexture, Texture2D playerTexture)
    {
        var x = playerTexture.width / 2 - circleTexture.width / 2;
        var y = playerTexture.height / 2 - circleTexture.height / 2;
        var playerColors = playerTexture.GetPixels(x, y, circleTexture.width, circleTexture.height);

        var circleColors = circleTexture.GetPixels();
        var newTextureColors = new Color[circleColors.Length];

        for (int i = 0; i < circleTexture.width * circleTexture.height; i++)
        {
            if (circleColors[i].a == 0)
            {
                newTextureColors[i] = Color.clear;
            }
            else
            {
                newTextureColors[i] = playerColors[i];
            }
        }

        playerTexture.Resize(circleTexture.width, circleTexture.height);
        playerTexture.SetPixels(newTextureColors);
        playerTexture.Apply();

        newTextureColors = null;
        playerColors = null;
        circleColors = null;
    }

    public static void SplitTextures(ref Texture2D backgroundTexture, Texture2D foregroundTexture, int x, int y)
    {
        var foregroundTextureColors = foregroundTexture.GetPixels();
        var backgroundTextureColors = backgroundTexture.GetPixels(x,y, foregroundTexture.width, foregroundTexture.height);
        var colors = new Color[foregroundTextureColors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            if(foregroundTextureColors[i].a == 0)
            {
                colors[i] = backgroundTextureColors[i];
            }
            else
            {
                colors[i] = foregroundTextureColors[i];
            }
        }

        backgroundTexture.SetPixels(x, y, foregroundTexture.width, foregroundTexture.height, colors);
        backgroundTexture.Apply();

        colors = null;
        foregroundTextureColors = null;
        backgroundTextureColors = null;
    }

    public static void ColorizeTexture(ref Texture2D texture, Color color)
    {
        var textureColor = texture.GetPixels();

        for (int i = 0; i < textureColor.Length; i++)
        {
            if(textureColor[i].a > 0)
            {
                textureColor[i] *= color;
            }
        }

        texture.SetPixels(textureColor);
        texture.Apply();
    }

    public static Texture2D GetGraphicTexture(Texture text)
    {
        Texture mainTexture = text;
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 24);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;

        return texture2D;
    }

}
