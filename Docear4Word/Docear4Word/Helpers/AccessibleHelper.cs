using System;
using System.Runtime.InteropServices;

using Office;

using Word;

namespace Docear4Word
{
	[ComVisible(false)]
	public static class AccessibleHelper
	{
		public static IAccessible GetTabByName(string tabName, _Application wordApp)
		{
			IAccessible result = null;
			var ribbon = wordApp.CommandBars["Ribbon"];
			var ribbona = ribbon as IAccessible;

			try
			{
				EnumerateChildren(ribbona, accessibles =>
					{
						for (var i = 0; i < accessibles.Length; i++)
						{
							var child = accessibles[i];
							if (!Marshal.IsComObject(child)) continue;

							if (result == null && HasName(child, tabName))
							{
								result = child;
								break;
							}
						}

						return result == null;
					});
			}
			catch
			{}
			finally
			{
				if (ribbon != null) Marshal.ReleaseComObject(ribbon);
				if (ribbona != null) Marshal.ReleaseComObject(ribbona);
			}

			return result;
		}

		static bool HasName(IAccessible accessible, string name)
		{
			try
			{
				if (!Marshal.IsComObject(accessible)) return false;

				return accessible.accName[0] == name;
			}
			catch
			{
				return false;
			}
		}

		static int GetChildCount(IAccessible accessible)
		{
			try
			{
				if (!Marshal.IsComObject(accessible)) return 0;

				return accessible.accChildCount;
			}
			catch
			{
				return 0;
			}
		}

		static void EnumerateChildren(IAccessible parent, Func<IAccessible[], bool> callback)
		{
			var childCount = GetChildCount(parent);
			if (childCount == 0) return;

			var children = new IAccessible[childCount];
			var obtainedCount = 0;

			AccessibleChildren(parent, 0, childCount, children, ref obtainedCount);

			if (callback(children))
			{
				for (var i = 0; i < children.Length; i++)
				{
					var child = children[i];
					if (!Marshal.IsComObject(child)) continue;

					EnumerateChildren(child, callback);

					Marshal.ReleaseComObject(child);
				}
			}
		}

        [DllImport("oleacc.dll")]
        public static extern int AccessibleChildren(IAccessible paccContainer, int iChildStart, int cChildren,
                                                     [Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] object[] rgvarChildren, ref int pcObtained);
	}
}