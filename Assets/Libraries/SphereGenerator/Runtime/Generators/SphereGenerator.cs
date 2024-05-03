using LazySquirrelLabs.SphereGenerator.Data;
using LazySquirrelLabs.SphereGenerator.Fragmentation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	/// <summary>
	/// Base class for all shape generators.
	/// </summary>
	internal abstract class SphereGenerator
	{
		#region Fields

		private const int MaxVertexCountUInt16 = 65_535;

		/// <summary>
		/// The radius of the generated sphere (i.e. the distance between each vertex and the sphere's center).
		/// </summary>
		private readonly float _radius;

		private readonly ushort _depth;

		private readonly bool _fragment;

		#endregion

		#region Properties

		private protected abstract NativeArray<int> Indices { get; }

		private protected abstract NativeArray<Vector3> Vertices { get; }

		private protected Allocator Allocator { get; }

		#endregion

		#region Setup

		private protected SphereGenerator(float radius, ushort depth, Allocator allocator)
		{
			_radius = radius;
			_fragment = true;
			_depth = depth;
			Allocator = allocator;
		}

		private protected SphereGenerator(float radius, Allocator allocator)
		{
			_radius = radius;
			_fragment = false;
			Allocator = allocator;
		}

		#endregion

		#region Internal

		internal Mesh Generate()
		{
			using var basicMeshData = new MeshData(Vertices, Indices);
			MeshData finalMeshData;

			if (_fragment)
			{
				using var newMeshData = MeshFragmenter.Fragment(basicMeshData, _depth, Allocator);
				finalMeshData = newMeshData;
			}
			else
			{
				finalMeshData = basicMeshData;
			}

			finalMeshData.SetRadius(_radius);
			var mesh = new Mesh();

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