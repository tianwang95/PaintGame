using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Props {
	public static class Tags {
		public static string Group = "Group";
		public static string Object = "Object";
		public static string Reset = "ResetManager";
		public static string Persist = "Persist";
		public static string Player = "Player";
		public static string ReticleCanvas = "ReticleCanvas";
	}

	public static class CanvasTriggers {
		public static string ExpandReticle = "ExpandTrigger";
		public static string ContractReticle = "ContractTrigger";
	}

	public static class PaintBucketTriggers {
		public static string TiltUp = "TiltUpTrigger";
		public static string TiltDown = "TiltDownTrigger";
	}

	public static class PaintBucketNames {
		public static string FullBucket = "FullBucket";
		public static string EmptyBucket = "EmptyBucket";
	}

	public static class WeaponPickupNames {
		public static string WeaponHolder = "WeaponHolder";
	}
}