using LazySquirrelLabs.SphereGenerator.Data;
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

		private readonly ushort _fragmentationDepth;

		#endregion

		#region Properties

		private protected abstract int[] Indices { get; }
		
		private protected abstract Vector3[] Vertices { get; }

		#endregion
		
		#region Setup

		private protected SphereGenerator(float radius, ushort fragmentationDepth)
		{
			_radius = radius;
			_fragmentationDepth = fragmentationDepth;
		}

		#endregion

		#region Internal

		internal Mesh Generate(Allocator allocator)
		{
			var vertices = new NativeArray<Vector3>(Vertices, allocator);

			for (var i = 0; i < Vertices.Length; i++)
			{
				vertices[i] = Vertices[i] * _radius;
			}

			var indices = new NativeList<int>(Indices.Length, allocator);

			foreach (var t in Indices)
			{
				indices.Add(t);
			}

			using var mashData = new MeshData(vertices, indices);
			using var newMeshData = MeshFragmenter.Fragment(mashData, _fragmentationDepth, Allocator.Temp);
			newMeshData.SetRadius(_radius);
			var mesh = new Mesh();
			if (newMeshData.Vertices.Length > MaxVertexCountUInt16)
				mesh.indexFormat = IndexFormat.UInt32;
			mesh.SetVertices(newMeshData.Vertices);
			mesh.SetIndices(newMeshData.Indices, MeshTopology.Triangles, 0);
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			return mesh;
		}

		#endregion
	}
}