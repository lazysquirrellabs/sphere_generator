using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	internal class CubeSphereGenerator : SphereGenerator
	{
		#region Fields

		private static readonly int[] CubeSphereIndices =
		{
			0, 1, 2,
			0, 2, 3,
			0, 4, 1,
			4, 5, 1,
			7, 5, 6,
			6, 5, 4,
			0, 3, 6,
			6, 4, 0,
			1, 5, 7,
			1, 7, 2,
			2, 7, 3,
			7, 6, 3
		};

		private static readonly Vector3[] CubeSphereVertices=
		{
			new(-0.5f,  0.5f, -0.5f), // 0
			new( 0.5f,  0.5f, -0.5f), // 1
			new( 0.5f, -0.5f, -0.5f), // 2
			new(-0.5f, -0.5f, -0.5f), // 3
			new(-0.5f,  0.5f,  0.5f), // 4
			new( 0.5f,  0.5f,  0.5f), // 5
			new(-0.5f, -0.5f,  0.5f), // 6
			new( 0.5f, -0.5f,  0.5f)  // 7
		};
		
		#endregion
		
		#region Properties


		private protected override int[] Indices => CubeSphereIndices;
		private protected override Vector3[] Vertices => CubeSphereVertices;

		#endregion
		
		#region Setup

		internal CubeSphereGenerator(float radius, ushort fragmentationDepth) : base(radius, fragmentationDepth) { }

		#endregion
	}
}