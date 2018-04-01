using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpHelper;

namespace Strogach.ObjectFigures
{
    class Table : IModels
    {
        private SharpMesh _meshModel;

        private ShaderResourceView _textureShader;

        private SharpShader _shader;

        private SharpDX.Direct3D11.Buffer _buffer;

        public Table(SharpDevice device)
        {
            _meshModel = SharpMesh.CreateFromObj(
                device,
                "D:\\CodeGitHub\\OhMyWoodWorkerSimulator\\Strogach\\ModelsResources\\Wooden Table 1.obj");

            _shader = new SharpShader(
                device,
                "D:\\CodeGitHub\\OhMyWoodWorkerSimulator\\Strogach\\ModelsResources\\objPlusImage.hlsl",
                new SharpShaderDescription()
                {
                    VertexShaderFunction = "VS",
                    PixelShaderFunction = "PS"
                },
                new InputElement[] 
                {
                    new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
                });

            _buffer = _shader.CreateBuffer<Camera>();

            _textureShader = device.LoadTextureFromFile(
                "D:\\CodeGitHub\\OhMyWoodWorkerSimulator\\Strogach\\ModelsResources\\Wooden_Table_1_default.png");
        }

        public void drawModel(SharpDevice device, Camera camera)
        {
            _shader.Apply();

            device.DeviceContext.PixelShader.SetShaderResource(1, _textureShader);

            device.DeviceContext.VertexShader.SetConstantBuffer(0, _buffer);
            device.DeviceContext.PixelShader.SetConstantBuffer(0, _buffer);

            device.UpdateData<Camera>(_buffer, camera);

            _meshModel.Begin();
            for (int i = 0; i < _meshModel.SubSets.Count; i++)
            {
                device.DeviceContext.PixelShader.SetShaderResource(0, _meshModel.SubSets[i].DiffuseMap);
                _meshModel.Draw(i);
            }

        }

        public void prepareModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _meshModel.Dispose();
            _shader.Dispose();
            _buffer.Dispose();
        }
    }
}
