using LazySquirrelLabs.SphereGenerators.Data;
using LazySquirrelLabs.SphereGenerators.Fragmentation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace LazySquirrelLabs.SphereGenerators.Generators
{
	/// <summary>
	/// Base class for all shape generators.
	/// </summary>
	public abstract class SphereGenerator
	{
		#region Fields

		private const int MaxVertexCountUInt16 = 65_535;

		/// <summary>
		/// The radius of the generated sphere (i.e. the distance between each vertex and the sphere's center).
		/// </summary>
		private readonly float _radius;

		private readonly ushort _depth;

		private readonly bool _fragment;

		private readonly string _meshName;

		#endregion

		#region Properties

		private protected abstract NativeArray<int> Indices { get; }

		private protected abstract NativeArray<Vector3> Vertices { get; }

		#endregion

		#region Setup

		private protected SphereGenerator(float radius, ushort depth, string meshName)
		{
			_radius = radius;
			_fragment = true;
			_depth = depth;
			_meshName = meshName;
		}

		private protected SphereGenerator(float radius, string meshName)
		{
			_radius = radius;
			_fragment = false;
			_meshName = meshName;
		}

		#endregion

		#region Internal

		internal Mesh Generate()
		{
			using var basicMeshData = new MeshData(Vertices, Indices);
			MeshData finalMeshData;

			if (_fragment)
			{
				using var newMeshData = MeshFragmenter.Fragment(basicMeshData, _depth, Allocator.Temp);
				finalMeshData = newMeshData;
			}
			else
			{
				finalMeshData = basicMeshData;
			}

			finalMeshData.SetRadius(_radius);
			var mesh = new Mesh();
			mesh.name = _meshName;

			if (finalMeshData.Vertices.Length > MaxVertexCountUInt16)
			{
				mesh.indexFormat = IndexFormat.UInt32;
			}

			mesh.SetVertices(finalMeshData.Vertices);
			mesh.SetIndices(finalMeshData.Indices, MeshTopology.Triangles, 0);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			return mesh;
		}

		#endregion
	}
}