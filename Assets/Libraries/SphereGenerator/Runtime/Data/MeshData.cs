using System;
using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Data
{
	internal struct MeshData : IDisposable
	{
		#region Fields

		private NativeArray<int> _indices;

		private NativeArray<Vector3> _vertices;

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