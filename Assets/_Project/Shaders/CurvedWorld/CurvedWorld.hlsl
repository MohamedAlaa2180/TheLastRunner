#ifndef CURVED_WORLD_INCLUDED
#define CURVED_WORLD_INCLUDED

// Fed by CurvedWorldOrigin.cs through Shader.SetGlobalVector.
// Declared outside UnityPerMaterial so SRP Batcher compatibility is preserved.
float4 _CurveOrigin;
float4 _CurveAmount; // x: bend along world Z, y: bend along world X

float3 CurveWorldPosition(float3 positionWS)
{
    float3 delta = positionWS - _CurveOrigin.xyz;
    positionWS.y -= delta.z * delta.z * _CurveAmount.x + delta.x * delta.x * _CurveAmount.y;
    return positionWS;
}

float3 CurveObjectPosition(float3 positionOS)
{
    float3 positionWS = TransformObjectToWorld(positionOS);
    return TransformWorldToObject(CurveWorldPosition(positionWS));
}

#endif
