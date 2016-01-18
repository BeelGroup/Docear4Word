using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Word;

namespace Docear4Word
{
	[ComVisible(false)]
	public class SelectionManager: IDisposable
	{
		readonly DocumentController documentController;
		readonly Range range;

		List<FieldMatch> fieldMatches;

		public SelectionManager(DocumentController documentController)
		{
			this.documentController = documentController;

			range = documentController.Document.Application.Selection.Range.Duplicate;
		}

		public int FieldMatchCount
		{
			get { return FieldMatches.Count; }
		}

		public FieldMatch FirstFieldMatch
		{
			get { return FieldMatchCount == 0 ? null : fieldMatches[0]; }
		}

		public List<FieldMatch> FieldMatches
		{
			get
			{
				if (fieldMatches != null) return fieldMatches;

				fieldMatches = new List<FieldMatch>();

var sw = Stopwatch.StartNew();
				//var cslFields = documentController.GetAllCSLFields();
				var cslFields = documentController.GetMaxTwoSelectedCSLFields(range.Start, range.End);
sw.Stop();
Debug.WriteLine(string.Format("Time to get {0} CSL fields = {1} ms", cslFields.Count,  sw.ElapsedMilliseconds));
				if (cslFields.Count == 0) return fieldMatches;
sw = Stopwatch.StartNew();
				var rangeStart = range.Start;
				var rangeEnd = range.End;

				foreach (var cslField in cslFields)
				{
					var fieldEnd = cslField.Result.End;
					if (fieldEnd < rangeStart) continue; // Field completely before selection

					Debug.Assert(cslField.Code.Start < cslField.Result.Start);
					var fieldStart = cslField.Code.Start;

					var isExactlyBeforeField = rangeEnd + 1 == fieldStart && rangeEnd == rangeStart;
					if (fieldStart > rangeEnd && // Field completely after selection
					    // But not immediately following a single point
					    !isExactlyBeforeField
						) break;

					FieldMatchType fieldMatchType;

					if (rangeStart < fieldStart)
					{
						fieldMatchType = isExactlyBeforeField
							? FieldMatchType.Prior
							: rangeEnd <= fieldEnd
								? FieldMatchType.Partial
								: FieldMatchType.Wrap;
					}
					else
					{
						fieldMatchType = rangeEnd <= fieldEnd
							? FieldMatchType.Inside
							: FieldMatchType.Partial;
					}

					if (fieldMatchType == FieldMatchType.None) continue;

					Debug.WriteLine(fieldMatchType);
					fieldMatches.Add(new FieldMatch(cslField, fieldMatchType, DocumentController.IsBibliographyField(cslField)));
				}
sw.Stop();
Debug.WriteLine("Time to process fields = " + sw.ElapsedMilliseconds + "ms");
				return fieldMatches;
			}
		}

		public bool IsNoFieldsSelected
		{
			get { return range.Fields.Count == 0; }
		}

		public bool IsAtOrBeforeSingleCitation
		{
			get { return FieldMatchCount == 1 && !FirstFieldMatch.IsBibliography; }
		}

		public bool IsInBibliography
		{
			get
			{
				foreach (var fieldMatch in FieldMatches)
				{
					if (fieldMatch.IsBibliography) return true;
				}

				return false;
			}
		}

		public void Dispose()
		{
			if (range != null) Marshal.ReleaseComObject(range);

			if (fieldMatches != null)
			{
				foreach (var fieldMatch in fieldMatches)
				{
					Marshal.ReleaseComObject(fieldMatch.Field);
				}
			}
		}
	}
}