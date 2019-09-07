    Shader "Unlit/Texture BR TR STR"
    {
        Properties
        {
            _MainTex ("Texture", 2D) = "white" {}
            _Brightness("Brightness", Range(0,1)) = 1
            _Transparency("Transparency", Range(0,1)) = 1
            _Saturation("Saturation", Range(0,1)) = 1
            
            _OutlineColor ("Outline Color", Color) = (1,1,1,1)
            _OutlineThickness("Outline Thickness", Range(0,100)) = 1
            _OutlineExpand("Outline Expand", Range(1,2)) = 1
        }
        SubShader
        {
            Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"  }
            LOD 100
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha 
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog alpha:fade
                
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                
                fixed4 _OutlineColor;
                float _OutlineThickness;
                float _OutlineExpand;

                
                v2f vert (appdata v)
                {
/*                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;*/
                    
                    v2f o;
                    // Slightly enlarge our quad, so we have a margin around it to draw the outline.
    //                float expand = 1.1f;
    //                float expand = 1.0f;
                    v.vertex.xyz *= _OutlineExpand;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    // If we want to get fancy, we could compute the expansion 
                    // dynamically based on line thickness & view angle, but I'm lazy)
                
                    // Expand the texture coordinate space by the same margin, symmetrically.
                    o.uv = (v.uv - 0.5f) * _OutlineExpand + 0.5f;
                    return o;                    
                }
                
                float _Transparency;
                float _Brightness;
                float _Saturation;
                


                fixed4 frag (v2f i) : SV_Target
                {
                    float2 fromCenter = abs(i.uv - 0.5f);
                    // Signed distance from the horizontal & vertical edges.
                    float2 fromEdge = fromCenter - 0.5f;
                
                    // Use screenspace derivatives to convert to pixel distances.
                    fromEdge.x /= length(float2(ddx(i.uv.x), ddy(i.uv.x)));
                    fromEdge.y /= length(float2(ddx(i.uv.y), ddy(i.uv.y)));
                
                    // Compute a nicely rounded distance from the edge.
                    float distance = abs(min(max(fromEdge.x,fromEdge.y), 0.0f) + length(max(fromEdge, 0.0f)));
                
                    // Clip out the part of the texture outside our original 0...1 UV space.
                    fixed4 col = tex2D(_MainTex, i.uv) * _Brightness;
                    
                    float nb = (col.r + col.g + col.b ) /3;
                    col.r = col.r * (_Saturation) + nb * (1-_Saturation);
                    col.g = col.g * (_Saturation) + nb * (1-_Saturation);
                    col.b = col.b * (_Saturation) + nb * (1-_Saturation);
                    col.a = _Transparency;
                    
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);                    
                    
                    col.a *= step(max(fromCenter.x, fromCenter.y), 0.5f);
                
                    // Blend in our outline within a controllable thickness of the edge.
                    col = lerp(col, _OutlineColor, saturate(_OutlineThickness - distance));
                
                    return col;
                }
                ENDCG
            }
        }
    }
