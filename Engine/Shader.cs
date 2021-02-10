using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.Engine
{
    public abstract class Shader
    {
        public delegate void    VertexProgram(FatVertex src, ref FatVertex dest);
        public delegate Color   FragmentProgram(FatVertex vertex);

        public abstract void Setup(Material material);

        public abstract VertexProgram GetVertexProgram();
        public abstract FragmentProgram GetFragmentProgram();
    }
}
