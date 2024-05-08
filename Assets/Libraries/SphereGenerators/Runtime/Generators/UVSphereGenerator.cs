using System;
using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerators.Generators
{
	/// A sphere generator that uses a UV sphere as basic shape.
	public class UVSphereGenerator : SphereGenerator
	{
		#region Setup

		/// <summary>
		/// <see cref="UVSphereGenerator"/>'s constructor.
		/// </summary>
		/// <param name="radius">The radius of the generated UV spheres.</param>
		/// <param name="depth">The fragmentation depth of the generated spheres. In order words, how many times the
		/// basic shape will be fragmented to form the sphere mesh. The larger the value, the greater the level of
		/// detail will be (more triangles and vertices) and the longer the generation process takes.
		/// It must be equal or greater than 3.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="depth"/> is
		/// lower than 3.</exception>
		public UVSphereGenerator(float radius, ushort depth) : base(radius, "UV Sphere")
		{
			if (depth < 3)
			{
				throw new ArgumentOutOfRangeException(nameof(depth), "UV sphere depth must be greater than 3.");
			}

			// The depth represents both the number of polar and azimuthal slices that the generated sphere will have.
			var polarDelta = Mathf.PI / depth;   // Polar slices are applied from 0 to 180 degrees.
			var azimuthalDelta = 2 * polarDelta; // Azimuthal slices are applied from 0 to 360 degrees.
			var vertices = GetVertexBuffer(depth, Allocator.Temp);
			var indices = GetIndicesBuffer(depth, Allocator.Temp);
			
			// Add the first vertex: the "top" of the sphere.
			vertices[0] = Vector3.up; 
			var vertexIx = 1;
			var indicesIx = 0;

			// Add all the triangles, except the ones in the bottom pole (it's a special case).
			for (var polarStep = 1; polarStep < depth; polarStep++)
			{
				for (var azimuthalStep = 0; azimuthalStep < depth; azimuthalStep++)
				{
					var addSecondTriangle = polarStep != 1;
					AddPrimitive(polarStep, azimuthalStep, addSecondTriangle);
				}
			}

			// Add the last triangles: the ones at the "bottom" of the sphere.
			vertices[vertexIx] = Vector3.down;
			var indexOfFirstVertexFromLastSlice = vertexIx - depth;

			for (var azimuthalStep = 0; azimuthalStep < depth; azimuthalStep++)
			{
				var secondIx = indexOfFirstVertexFromLastSlice + azimuthalStep;
				var thirdIx = azimuthalStep == depth - 1 ? indexOfFirstVertexFromLastSlice : secondIx + 1;
				AddTriangle(vertexIx, secondIx, thirdIx, indices, ref indicesIx);
			}

			Vertices = vertices;
			Indices = indices;
			return;

			static NativeArray<Vector3> GetVertexBuffer(ushort fragmentationDepth, Allocator allocator)
			{
				var sliceVertexCount = (fragmentationDepth - 1) * fragmentationDepth; // Polar slices * azimuthal slices
				var totalVertexCount = sliceVertexCount + 2; // Polar and azimuthal slices + poles.
				return new NativeArray<Vector3>(totalVertexCount, allocator, NativeArrayOptions.UninitializedMemory);
			}

			static NativeArray<int> GetIndicesBuffer(ushort fragmentationDepth, Allocator allocator)
			{
				// First, find the triangle count for the poles. Each pole has exactly D triangles, where D is the
				// fragmentation depth. There are 2 poles (top and bottom).
				var polesTriangleCount = 2 * fragmentationDepth;
				
				// Then, find the quad count for the "middle" of the sphere (everything except the poles). It can be
				// found by calculating the number of vertical slices (which is the same as the depth) by the number of
				// horizontal slices, except the poles (-2).
				var middleQuadCount = fragmentationDepth * (fragmentationDepth - 2);
				// Each quad is made out of 2 triangles.
				var middleTriangleCount = 2 * middleQuadCount;
				
				// Finally, find the total triangle count.
				var triangleCount = middleTriangleCount + polesTriangleCount;
				var indicesCount = triangleCount * 3;
				return new NativeArray<int>(indicesCount, allocator, NativeArrayOptions.UninitializedMemory);
			}

			static void AddTriangle(int ix1, int ix2, int ix3, NativeArray<int> indices, ref int indicesIx)
			{
				indices[indicesIx] = ix1;
				indicesIx++;
				indices[indicesIx] = ix2;
				indicesIx++;
				indices[indicesIx] = ix3;
				indicesIx++;
			}

			void AddPrimitive(int polarStep, int azimuthalStep, bool isQuad)
			{
				// Calculate the spherical coordinates.
				var polar = polarDelta * polarStep;
				var azimuth = azimuthalDelta * azimuthalStep;
				// Convert to cartesian.
				vertices[vertexIx] = PolarToCartesian(polar, azimuth);

				// We need 3 points to form the triangle.
				var secondVertexIx = GetVertexAboveIndex(polarStep, vertexIx, depth);
				var thirdVertexIx = GetNextVertexInPolarSliceIndex(azimuthalStep, vertexIx, depth);

				AddTriangle(vertexIx, secondVertexIx, thirdVertexIx, indices, ref indicesIx);

				if (isQuad)
				{
					var fourthVertexIx = GetVertexAboveNextIndex(polarStep, azimuthalStep, vertexIx, depth);
					AddTriangle(secondVertexIx, fourthVertexIx, thirdVertexIx, indices, ref indicesIx);
				}

				vertexIx++;
				return;

				static Vector3 PolarToCartesian(float polar, float azimuth)
				{
					var polarSin = Mathf.Sin(polar);
					var x = polarSin * Mathf.Cos(azimuth);
					var y = Mathf.Cos(polar);
					var z = polarSin * Mathf.Sin(azimuth);
					return new Vector3(x, y, z);
				}

				static int GetVertexAboveIndex(int polarStep, int vertexIx, int slices)
				{
					if (polarStep == 1)
					{
						return 0;
					}

					return vertexIx - slices;
				}

				static int GetNextVertexInPolarSliceIndex(int azimuthStep, int vertexIx, int slices)
				{
					if (azimuthStep == slices - 1)
					{
						return vertexIx - (slices - 1);
					}

					return vertexIx + 1;
				}

				static int GetVertexAboveNextIndex(int polarStep, int azimuthStep, int vertexIx, int slices)
				{
					var nextIx = GetNextVertexInPolarSliceIndex(azimuthStep, vertexIx, slices);
					return GetVertexAboveIndex(polarStep, nextIx, slices);
				}
			}
		}

		#endregion

		#region Properties

		private protected override NativeArray<int> Indices { get; }

		private protected override NativeArray<Vector3> Vertices { get; }

		#endregion
	}
}