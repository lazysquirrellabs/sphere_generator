using System;
using LazySquirrelLabs.SphereGenerator.Data;
using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Fragmentation
{
	/// <summary>
	/// Fragments a mesh into sub-triangles given an arbitrary depth. The original mesh is modified.
	/// </summary>
	internal static class MeshFragmenter
	{
		#region Internal

		internal static MeshData Fragment(MeshData meshData, ushort fragmentationDepth, Allocator allocator)
		{
			if (fragmentationDepth == 0)
			{
				return meshData;
			}

			// Each triangle has 3 indices, so divide the number of indices by 3 to find the number of triangles
			var initialTriangleCount = meshData.Indices.Length / 3;
			// Find the number of triangles in the final mesh (at maximum depth)
			var finalTriangleCount = GetTriangleCountForDepth(initialTriangleCount, fragmentationDepth);
			var finalIndicesCount = finalTriangleCount * 3;
			var finalVertexCount = finalIndicesCount / 2;

			// Create temporary index and vertex buffers
			var indicesBuffer1 = CreateNativeArray<int>(finalIndicesCount, allocator);
			NativeArray<int>.Copy(meshData.Indices, indicesBuffer1, meshData.Indices.Length);
			var indicesBuffer2 = CreateNativeArray<int>(finalIndicesCount, allocator);
			var verticesBuffer1 = CreateNativeArray<Vector3>(finalVertexCount, allocator);
			NativeArray<Vector3>.Copy(meshData.Vertices, verticesBuffer1, meshData.Vertices.Length);
			var verticesBuffer2 = CreateNativeArray<Vector3>(finalVertexCount, allocator);

			// Instead of creating a new array for each depth, we use the same 2 arrays everywhere: 1 for reading and
			// one for writing. Both have exactly the number of elements necessary for the final depth, but they will
			// never (except for the last depth) be fully utilized. Every time we step into a new depth, we swap them
			// to read from the last depth's write array. The final "read" arrays will hold the desired data.
			var readIndices = indicesBuffer1;
			var writeIndices = indicesBuffer2;
			var readVertices = verticesBuffer1;
			var writeVertices = verticesBuffer2;
			double currentDepthTotalTriangles = initialTriangleCount;

			for (var depthCount = 1; depthCount <= fragmentationDepth; depthCount++)
			{
				FragmentAllTrianglesForDepth(depthCount, currentDepthTotalTriangles);
			}

			writeIndices.Dispose();
			writeVertices.Dispose();

			return new MeshData(readVertices, readIndices);

			void FragmentAllTrianglesForDepth(int depth, double triangleCount)
			{
				for (var i = 0; i < triangleCount; i++)
				{
					FragmentTriangle(i, readIndices, writeIndices, readVertices, writeVertices);
				}

				currentDepthTotalTriangles = GetTriangleCountForDepth(initialTriangleCount, depth);
				// Swap read and write arrays
				(readIndices, writeIndices) = (writeIndices, readIndices);
				(readVertices, writeVertices) = (writeVertices, readVertices);
			}

			static int GetTriangleCountForDepth(int initialTriangleCount, int depth)
			{
				return (int)(Math.Pow(4, depth) * initialTriangleCount);
			}

			static NativeArray<T> CreateNativeArray<T>(int length, Allocator allocator) where T : unmanaged
			{
				return new NativeArray<T>(length, allocator, NativeArrayOptions.UninitializedMemory);
			}

			static void FragmentTriangle(int triangleIx, NativeArray<int> readTriangles,
			                             NativeArray<int> writeTriangles, NativeArray<Vector3> readVertices, 
			                             NativeArray<Vector3> writeVertices)
			{
				// Each original triangle has 3 vertices, so we need to offset that from reading
				var readTriangleIx = triangleIx * 3;
				// The fragmented triangle will have 4 triangles, each one with 3 vertices, so we need to offset by 12
				var writeTriangleIx = triangleIx * 12;
				// The fragmented triangle will have 6 vertices, so we need to offset by 6
				var writeVertexIx = triangleIx * 6;

				// Read the original vertex data
				var indexVertex1 = readTriangles[readTriangleIx];
				var indexVertex2 = readTriangles[readTriangleIx + 1];
				var indexVertex3 = readTriangles[readTriangleIx + 2];

				// Calculate the new vertices
				var v1 = readVertices[indexVertex1];
				var v2 = readVertices[indexVertex2];
				var v3 = readVertices[indexVertex3];
				var v4 = (v1 + v2) / 2;
				var v5 = (v2 + v3) / 2;
				var v6 = (v3 + v1) / 2;

				// Add new vertices
				var ix1 = AddVertex(v1);
				var ix2 = AddVertex(v2);
				var ix3 = AddVertex(v3);
				var ix4 = AddVertex(v4);
				var ix5 = AddVertex(v5);
				var ix6 = AddVertex(v6);

				// Add the new triangles using the indices of the new vertices
				AddTriangle(ix1, ix4, ix6);
				AddTriangle(ix4, ix5, ix6);
				AddTriangle(ix5, ix3, ix6);
				AddTriangle(ix4, ix2, ix5);
				return;

				int AddVertex(Vector3 v)
				{
					writeVertices[writeVertexIx] = v;
					var index = writeVertexIx;
					writeVertexIx++;
					return index;
				}

				void AddTriangle(int i1, int i2, int i3)
				{
					writeTriangles[writeTriangleIx] = i1;
					writeTriangles[writeTriangleIx + 1] = i2;
					writeTriangles[writeTriangleIx + 2] = i3;
					writeTriangleIx += 3;
				}
			}
		}

		#endregion
	}
}