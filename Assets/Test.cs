using UnityEngine;
using Unity.Barracuda;

sealed class Test : MonoBehaviour
{
    [SerializeField] Texture2D _source = null;
    [SerializeField] ComputeShader _preprocess = null;
    [SerializeField] NNModel _model = null;

    const int InputWidth = 256;
    const int InputHeight = 192;
    const int BufferLength = InputWidth * InputHeight * 3;

    ComputeBuffer _buffer;
    IWorker _worker;

    void Start()
    {
        _buffer = new ComputeBuffer(BufferLength, sizeof(float));

        _preprocess.SetTexture(0, "_Texture", _source);
        _preprocess.SetBuffer(0, "_Tensor", _buffer);
        _preprocess.SetInts("_Dims", InputWidth, InputHeight);
        _preprocess.Dispatch(0, InputWidth / 8, InputHeight / 8, 1);

        _worker = ModelLoader.Load(_model).CreateWorker();

    }

    void OnDestroy()
    {
        _buffer.Dispose();
        _worker.Dispose();
    }

    void Update()
    {
        using (var tensor = new Tensor(1, InputHeight, InputWidth, 3, _buffer))
            _worker.Execute(tensor);

        var output = _worker.PeekOutput();
        Debug.Log(output);
    }
}
