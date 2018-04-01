using SharpDX.DXGI;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach
{
    class ModelMesh : IDisposable
    {
        InputElement[] m_inputElements;
        public InputElement[] InputElements
        {
            set { m_inputElements = value; }
            get { return m_inputElements; }
        }

        InputLayout m_inputLayout;
        public InputLayout InputLayout
        {
            set { m_inputLayout = value; }
            get { return m_inputLayout; }
        }

        int m_vertexSize;
        public int VertexSize
        {
            set { m_vertexSize = value; }
            get { return m_vertexSize; }
        }

        SharpDX.Direct3D11.Buffer m_vertexBuffer;
        public SharpDX.Direct3D11.Buffer VertexBuffer
        {
            set { m_vertexBuffer = value; }
            get { return m_vertexBuffer; }
        }

        SharpDX.Direct3D11.Buffer m_indexBuffer;
        public SharpDX.Direct3D11.Buffer IndexBuffer
        {
            set { m_indexBuffer = value; }
            get { return m_indexBuffer; }
        }

        int m_vertexCount;
        public int VertexCount
        {
            set { m_vertexCount = value; }
            get { return m_vertexCount; }
        }

        int m_indexCount;
        public int IndexCount
        {
            set { m_indexCount = value; }
            get { return m_indexCount; }
        }

        int m_primitiveCount;
        public int PrimitiveCount
        {
            set { m_primitiveCount = value; }
            get { return m_primitiveCount; }
        }

        PrimitiveTopology m_primitiveTopology;
        public PrimitiveTopology PrimitiveTopology
        {
            set { m_primitiveTopology = value; }
            get { return m_primitiveTopology; }
        }

        Texture2D m_diffuseTexture;
        public Texture2D DiffuseTexture
        {
            set { m_diffuseTexture = value; }
            get { return m_diffuseTexture; }
        }

        ShaderResourceView m_diffuseTextureView;
        public ShaderResourceView DiffuseTextureView
        {
            set { m_diffuseTextureView = value; }
            get { return m_diffuseTextureView; }
        }

        //add texture and texture view for the shader
        public void AddTextureDiffuse(SharpDX.Direct3D11.Device device, string path)
        {
            //m_diffuseTexture = new Texture2D(device, new Texture2DDescription()
            //   {
            //        
            //    }
            //);
           // m_diffuseTexture = Texture
           m_diffuseTexture = new SharpDX.Direct3D11.Texture2D(device, new SharpDX.Direct3D11.Texture2DDescription()
           {
               Width = 512,
               Height = 512,
               ArraySize = 1,
               BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource | BindFlags.RenderTarget,
               Usage = SharpDX.Direct3D11.ResourceUsage.Default,
               CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
               Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
               MipLevels = 1,
               OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
               SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
           });

            m_diffuseTextureView = new ShaderResourceView(device, m_diffuseTexture);
        }

        //set the input layout and make sure it matches vertex format from the shader
        public void SetInputLayout(SharpDX.Direct3D11.Device device, ShaderSignature inputSignature)
        {
            m_inputLayout = new InputLayout(device, inputSignature, m_inputElements);
            if (m_inputLayout == null)
            {
                throw new Exception("mesh and vertex shader input layouts do not match!");
            }
        }

        //dispose D3D related resources
        public void Dispose()
        {
            m_inputLayout.Dispose();
            m_vertexBuffer.Dispose();
            m_indexBuffer.Dispose();
        }

    }
}
