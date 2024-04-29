using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	internal class UVSphereGenerator : SphereGenerator
	{
		#region

		private readonly ushort _slices;

		#endregion

		#region Setup

		internal UVSphereGenerator(float radius, ushort fragmentationDepth) : base(radius)
		{
			var polarDelta = Mathf.PI / fragmentationDepth;
			var azimuthalDelta = 2 * polarDelta;
			var vertices = GetVertexBuffer(fragmentationDepth);
			var indices = GetIndicesBuffer(fragmentationDepth);
			vertices[0] = Vector3.up;
			var vertexIx = 1;
			var indicesIx = 0;

			for (var polarStep = 1; polarStep < fragmentationDepth; polarStep++)
			{
				for (var azimuthalStep = 0; azimuthalStep < fragmentationDepth; azimuthalStep++)
				{
					var addSecondTriangle = polarStep != 1;
					AddVertex(polarStep, azimuthalStep, addSecondTriangle);
				}
			}
			
			vertices[vertexIx] = Vector3.down;
			var indexOfFirstVertexFromLastSlice = vertexIx - fragmentationDepth;

			for (var azimuthalStep = 0; azimuthalStep < fragmentationDepth; azimuthalStep++)
			{
				var secondIx = indexOfFirstVertexFromLastSlice + azimuthalStep;
				var thirdIx = azimuthalStep == fragmentationDepth - 1 ? indexOfFirstVertexFromLastSlice : secondIx + 1;
				AddTriangle(vertexIx, secondIx, thirdIx, indices, ref indicesIx);
			}
			
			Vertices = vertices;
			Indices = indices;
			return;

			static Vector3[] GetVertexBuffer(ushort fragmentationDepth)
			{
				var sliceVertexCount = (fragmentationDepth - 1) * fragmentationDepth; // Polar slices * azimuthal slices
				return new Vector3[sliceVertexCount + 2]; // Polar and azimuthal slices + poles.
			}

			static int[] GetIndicesBuffer(ushort fragmentationDepth)
			{
				var quadCount = (fragmentationDepth - 2) * fragmentationDepth;
				var polarTriangleCount = 2 * fragmentationDepth;
				var triangleCount = 2 * quadCount + polarTriangleCount;
				var indicesCount = triangleCount * 3;
				return new int[indicesCount];
			}
			
			static void AddTriangle(int ix1, int ix2, int ix3, IList<int> indices, ref int indicesIx)
			{
				indices[indicesIx] = ix1;
				indicesIx++;
				indices[indicesIx] = ix2;
				indicesIx++;
				indices[indicesIx] = ix3;
				indicesIx++;
			}
			
			void AddVertex(int polarStep, int azimuthalStep, bool addSecondTriangle)
			{
				var polar = polarDelta * polarStep;
				var azimuth = azimuthalDelta * azimuthalStep;
				vertices[vertexIx] = PolarToCartesian(polar, azimuth);
				// Debug.Log(vertices[vertexIx]);

				var secondVertexIx = GetVertexAboveIndex(polarStep, vertexIx, fragmentationDepth);
				var thirdVertexIx = GetVertexAboveNextIndex(polarStep, azimuthalStep, vertexIx, fragmentationDepth);
				var fourthVertexIx = GetNextVertexInPolarSliceIndex(azimuthalStep, vertexIx, fragmentationDepth);

				AddTriangle(vertexIx, secondVertexIx, fourthVertexIx, indices, ref indicesIx);
				if (addSecondTriangle)
					AddTriangle(secondVertexIx, thirdVertexIx, fourthVertexIx, indices, ref indicesIx);
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

		private protected override int[] Indices { get; }

		private protected override Vector3[] Vertices { get; }

		#endregion
	}
}