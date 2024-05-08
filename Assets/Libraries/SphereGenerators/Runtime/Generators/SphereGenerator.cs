using LazySquirrelLabs.SphereGenerators.Data;
using LazySquirrelLabs.SphereGenerators.Fragmentation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace LazySquirrelLabs.SphereGenerators.Generators
{
	/// <summary>
	/// Base class for all sphere generators.
	/// </summary>
	public abstract class SphereGenerator
	{
		#region Fields

		private const int MaxVertexCountUInt16 = 65_535;

		/// <summary>
		/// The radius of the generated sphere (i.e. the distance between each vertex and the sphere's center).
		/// </summary>
		private readonly float _radius;

		/// <summary>
		/// The fragmentation depth applied to the sphere's basic shape. Only relevant when fragmentation is used.
		/// </summary>
		private readonly ushort _depth;

		/// <summary>
		/// Whether this generator should fragment its basic shape.
		/// </summary>
		private readonly bool _fragment;

		/// <summary>
		/// The name of the generated mesh, representing the sphere's basic shape.
		/// </summary>
		private readonly string _meshName;

		#endregion

		#region Properties

		/// <summary>
		/// The vertices of the generated sphere mesh.
		/// </summary>
		private protected abstract NativeArray<Vector3> Vertices { get; }
		
		/// <summary>
		/// The indices of the generated sphere mesh's triangles.
		/// </summary>
		private protected abstract NativeArray<int> Indices { get; }

		#endregion

		#region Setup

		/// <summary>
		/// Creates a <see cref="SphereGenerator"/> that uses fragmentation.
		/// </summary>
		/// <param name="radius">The sphere's radius.</param>
		/// <param name="depth">The fragmentation depth that will be applie to the sphere's basic mesh.</param>
		/// <param name="meshName">The name of the generated mesh.</param>
		private protected SphereGenerator(float radius, ushort depth, string meshName)
		{
			_radius = radius;
			_fragment = true;
			_depth = depth;
			_meshName = meshName;
		}

		/// <summary>
		/// Creates a <see cref="SphereGenerator"/> that does not fragmentation.
		/// </summary>
		/// <param name="radius">The sphere's radius.</param>
		/// <param name="meshName">The name of the generated mesh.</param>
		private protected SphereGenerator(float radius, string meshName)
		{
			_radius = radius;
			_fragment = false;
			_meshName = meshName;
		}

		#endregion

		#region Public

		/// <summary>
		/// Generates a <see cref="Mesh"/> that represents a sphere.
		/// </summary>
		/// <returns>The mesh that represents the sphere, with its bounds, normals and tangents recalculated.</returns>
		public Mesh Generate()
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