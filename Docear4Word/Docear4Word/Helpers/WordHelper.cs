using System;

using Word;

namespace Docear4Word
{
	public static class WordHelper
	{
		const float PointsPerCentimeter = 28.35f;

		public static float CMToPoints(float cm)
		{
			return cm * PointsPerCentimeter;
		}

		public static Range GetEndOfRange(Range range)
		{
			var result = range.Duplicate;
			result.Start = result.End;

			return result;
		}
	}
}