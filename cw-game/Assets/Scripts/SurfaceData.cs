using UnityEngine;

// 1. Definimos los tipos de suelo disponibles
public enum SurfaceType
{
    Stone,
    Snow,
    Ice,
    Spike,
    ColapsedPlatform,
    Default
}

public class SurfaceData : MonoBehaviour
{
    // 2. Creamos la variable que aparecerá como lista en el Inspector
    public SurfaceType surfaceType;
}