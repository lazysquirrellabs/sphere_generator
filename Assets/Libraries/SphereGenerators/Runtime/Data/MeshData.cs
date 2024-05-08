using System;
using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerators.Data
{
	/// <summary>
	/// Simple entity that wraps a mesh's vertex and index data.
	/// </summary>
	internal struct MeshData : IDisposable
	{
		#region Fields

		private NativeArray<Vector3> _vertices;
		
		private NativeArray<int> _indices;

		#endregion

		#region Properties

		internal NativeArray<Vector3> Vertices => _vertices;

		internal NativeArray<int> Indices => _indices;

		#endregion

		#region Setup

		internal MeshData(NativeArray<Vector3> vertices, NativeArray<int> indices)
		{
			_vertices = vertices;
			_indices = indices;
		}

		#endregion

		#region Public

		public void Dispose()
		{
			_indices.Dispose();
			_vertices.Dispose();
		}

		#endregion

		#region Internal

		/// <summary>
		/// Set the radius of the sphere mesh, effectively putting all vertices at the same distance from the center.
		/// </summary>
		/// <param name="radius">How far all vertices should be from the center, in units.</param>
		internal void SetRadius(float radius)
		{
			for (var i = 0; i < _vertices.Length; i++)
			{
				_vertices[i] = _vertices[i].normalized * radius;
			}
		}

		#endregion
	}
}