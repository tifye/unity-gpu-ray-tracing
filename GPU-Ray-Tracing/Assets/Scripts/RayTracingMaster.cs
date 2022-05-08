using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingMaster : MonoBehaviour
{
    public ComputeShader RayTracingShader;
    public Texture SkyboxTexture;
    public Light DirectionalLight;

    private ComputeBuffer _sphereBuffer;
    private RenderTexture _target;
    private Camera _camera;
    private uint _currentSample = 0;
    private Material _addMaterial;
    private SceneGenerator _sceneGenerator;


    private void OnEnable() {
        ResetScene();
    }

    private void OnDisable() {
        if (_sphereBuffer != null) {
            _sphereBuffer.Release();
        }
    }


    public void ResetScene() {
        if (_sphereBuffer != null) {
            _sphereBuffer.Release();
        }
        _currentSample = 0;
        _sceneGenerator.SetUpScene();
        _sphereBuffer = new ComputeBuffer(_sceneGenerator.spheres.Count, 40);
        _sphereBuffer.SetData(_sceneGenerator.spheres);
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _sceneGenerator = GetComponent<SceneGenerator>();
    }

    private void Update() {
        if (transform.hasChanged || DirectionalLight.transform.hasChanged) {
            _currentSample = 0;
            transform.hasChanged = false;
            DirectionalLight.transform.hasChanged = false;
        }

    }

    private void InitRenderTexture() {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height) {
            // Release render texture if we already have one
            if (_target != null) {
                _target.Release(); // TODO: What does it mean to release
            }

            // Get a render target for Ray Tracing
            _currentSample = 0;
            _target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }

    // Called automatically by Unity when camera finishes rendering (if attached to a camera)
    // Destination is the screen
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        SetShaderParamters();
        Render(destination);
    }

    private void Render(RenderTexture destination) {
        // Make sure we have a current render target
        InitRenderTexture();

        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        // Set size of thread groups
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        if (_addMaterial == null) {
            _addMaterial = new Material(Shader.Find("Hidden/AddShader"));
        }
        _addMaterial.SetFloat("_Sample", _currentSample);
        // Blit the result texture to the screen
        Graphics.Blit(_target, destination, _addMaterial);
        _currentSample++;
    }

    private void SetShaderParamters() {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));
        RayTracingShader.SetBuffer(0, "_Spheres", _sphereBuffer);

        Vector3 l = DirectionalLight.transform.forward;
        RayTracingShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, DirectionalLight.intensity));
    }
}
