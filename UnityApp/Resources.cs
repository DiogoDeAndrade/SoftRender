using System.Collections.Generic;
using System.IO;
using SoftRender.Engine;
using Mathlib;

namespace SoftRender.UnityApp
{
    static public class Resources
    {
        static Dictionary<string, Resource> resources = new Dictionary<string, Resource>();

        public static T Load<T>(string filename) where T : Resource
        {
            Resource ret = null;

            try
            {
                // Select loader based on file extension
                string extension = Path.GetExtension(filename).ToLower();

                switch (extension)
                {
                    case ".obj":
                        ret = LoadObj(filename);
                        break;
                    case ".png":
                        ret = LoadPNG(filename);
                        break;
                    default:
                        Debug.Log("Unknown file type " + filename + "!");
                        break;
                }

                if (ret != null)
                {
                    resources.Add(ret.name, ret);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to load file " + filename + ":" + e.Message);
            }

            return ret as T;
        }

        static public void Add(string name, Resource ret)
        {
            resources.Add(ret.name, ret);
        }

        static public void Add(string name, Material mat)
        {
            var matRes = new MaterialResource(name, mat);

            resources.Add(name, matRes);
        }

        static Mesh LoadObj(string filename)
        {
            Mesh newMesh = new Mesh(filename);

            List<Vector3> objVertices = new List<Vector3>();
            List<Vector3> objNormals = new List<Vector3>();
            List<Vector2> objUV = new List<Vector2>();

            List<Vector3> meshVertex = new List<Vector3>();
            List<Vector3> meshNormal = new List<Vector3>();
            List<Vector2> meshUV = new List<Vector2>();

            List<int> triangles = new List<int>();

            Material defaultMaterial = null;

            string currentMaterial = "";

            using (var fs = File.OpenText(filename))
            {
#nullable enable
                string? line = "";
#nullable disable
                while (line != null)
                {
                    line = fs.ReadLine();
                    if (line == null) break;
                    if (line.Length == 0) continue;
                    line = line.Trim();
                    line = line.ToLower();

                    // Is a comment?
                    if (line[0] == '#') continue;

                    // Has tokens
                    var tokens = line.Split(' ');
                    if (tokens.Length == 0) continue;

                    var cmd = tokens[0];
                    switch (cmd)
                    {
                        case "mtllib":
                            // Load material library - Only supports the first one in the file, as we only support a single mesh
                            var baseDir = Path.GetFullPath(filename);
                            baseDir = Path.GetDirectoryName(baseDir);
                            defaultMaterial = LoadOBJ_LoadMateriaLibrary(baseDir + "/" + tokens[1]);
                            break;
                        case "o":
                            newMesh.name = tokens[1];
                            break;
                        case "v":
                            var v = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                            objVertices.Add(v);
                            break;
                        case "vt":
                            var vt = new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2]));
                            objUV.Add(vt);
                            break;
                        case "vn":
                            var vn = new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
                            objNormals.Add(vn);
                            break;
                        case "usemtl":
                            currentMaterial = tokens[1];
                            break;
                        case "s":
                            // Smooth shading, unused for now, we always use the normals that come with the file
                            break;
                        case "g":
                            Debug.Log("Multi-material mesh not supported, but still loaded as a single material mesh...");
                            break;
                        case "f":
                            // Triangles or quads?
                            int i1 = LoadObj_GetVertex(tokens[1], objVertices, objNormals, objUV, ref meshVertex, ref meshNormal, ref meshUV);
                            int i2 = LoadObj_GetVertex(tokens[2], objVertices, objNormals, objUV, ref meshVertex, ref meshNormal, ref meshUV);
                            int i3 = LoadObj_GetVertex(tokens[3], objVertices, objNormals, objUV, ref meshVertex, ref meshNormal, ref meshUV);

                            if (tokens.Length == 4)
                            {
                                // Triangles
                                triangles.Add(i1);
                                triangles.Add(i2);
                                triangles.Add(i3);
                            }
                            else if (tokens.Length == 5)
                            {
                                // Quads
                                int i4 = LoadObj_GetVertex(tokens[4], objVertices, objNormals, objUV, ref meshVertex, ref meshNormal, ref meshUV);

                                triangles.Add(i1);
                                triangles.Add(i2);
                                triangles.Add(i3);

                                triangles.Add(i1);
                                triangles.Add(i3);
                                triangles.Add(i4);
                            }
                            else
                            {
                                Debug.Log("N-sided faces not supported (N=" + (tokens.Length - 2) + ")");
                                // Don't support any other types of primitives
                                throw new System.NotImplementedException();
                            }
                            break;
                        case "l":
                            // Polyline, don't care
                            break;
                        default:
                            Debug.Log("Unknown token " + cmd);
                            break;
                    }
                }
            }

            newMesh.defaultMaterial = defaultMaterial;
            newMesh.SetVertices(meshVertex);
            newMesh.SetNormals(meshNormal);
            newMesh.SetUV(0, meshUV);
            newMesh.SetTriangles(triangles);

            return newMesh;
        }

        private static int LoadObj_GetVertex(string v, List<Vector3> objVertices, List<Vector3> objNormals, List<Vector2> objUV, ref List<Vector3> meshVertex, ref List<Vector3> meshNormal, ref List<Vector2> meshUV)
        {
            var indices = v.Split('/');

            Vector3 pos = objVertices[int.Parse(indices[0]) - 1];
            Vector3 normal = objNormals[int.Parse(indices[2]) - 1];
            Vector2 uv = objUV[int.Parse(indices[1]) - 1];

            // Check if this index is already on the mesh data
            for (int i = 0; i < meshVertex.Count; i++)
            {
                if ((meshVertex[i] == pos) &&
                    (meshNormal[i] == normal) &&
                    (meshUV[i] == uv))
                {
                    return i;
                }
            }

            // Add it
            meshVertex.Add(pos);
            meshNormal.Add(normal);
            meshUV.Add(uv);

            return meshVertex.Count - 1;
        }

        private static Material LoadOBJ_LoadMateriaLibrary(string filename)
        {
            // Ignore everything, we don't have a material model or shaders, but it might make sense to defined
            // a blender shader for this purpose

            Material newMaterial = null;

            using (var fs = File.OpenText(filename))
            {
#nullable enable
                string? line = "";
#nullable disable
                while (line != null)
                {
                    line = fs.ReadLine();
                    if (line == null) break;
                    if (line.Length == 0) continue;
                    line = line.Trim();
                    line = line.ToLower();

                    // Is a comment?
                    if (line[0] == '#') continue;

                    // Has tokens
                    var tokens = line.Split(' ');
                    if (tokens.Length == 0) continue;

                    var cmd = tokens[0];
                    /*switch (cmd)
                    {
                    }*/
                }
            }

            return newMaterial;
        }

        static Texture LoadPNG(string filename)
        {
            Texture ret = new Texture(filename);

            if (!ret.Load(filename))
            {
                Debug.Log("Failed to read texture " + filename + "!");
                return null;
            }

            return ret;
        }
    }
}
