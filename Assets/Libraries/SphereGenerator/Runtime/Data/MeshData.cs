using System;
using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Data
{
	/// <summary>
	/// Simple entity that wraps a mesh's vertex and index data.
	/// </summary>
	internal readonly struct MeshData : IDisposable
	{
		#region Properties

		internal NativeArray<Vector3> Vertices { get; }

		internal NativeArray<int> Indices { get; }

		#endregion

		#region Setup

		internal MeshData(NativeArray<Vector3> vertices, NativeArray<int> indices)
		{
			Vertices = vertices;
			Indices = indices;
		}

		#endregion

		#region Public

		public void Dispose()
		{
			Indices.Dispose();
			Vertices.Dispose();
		}

		#endregion
	}
}