using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Docear4Word
{
	[ComVisible(false)]
	public class BindingListAce<T>: BindingList<T>, IBindingListView
	{
		readonly List<T> items;
		PropertyComparerCollection<T> sortComparers;
		Predicate<T> filter;

		public BindingListAce(List<T> items)
		{
			this.items = items;

			FilterAndSort();
		}

		public int TotalItemCount
		{
			get { return items.Count; }
		}

		protected override bool IsSortedCore
		{
			get { return sortComparers != null; }
		}

		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get
			{
				return sortComparers == null
				       	? ListSortDirection.Ascending
				       	: sortComparers.PrimaryDirection;
			}
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return sortComparers == null ? null : sortComparers.PrimaryProperty; }
		}

		void FilterAndSort()
		{
			var previousValue = RaiseListChangedEvents;
			RaiseListChangedEvents = false;

			try
			{
				List<T> filteredItems;

				if (filter == null)
				{
					filteredItems = new List<T>(items);
				}
				else
				{
					filteredItems = new List<T>();

					foreach(var item in items)
					{
						if (filter(item))
						{
							filteredItems.Add(item);
						}
					}
				}

				if (sortComparers != null)
				{
					filteredItems.Sort(sortComparers);
				}

				Clear();

				foreach(var item in filteredItems)
				{
					Add(item);
				}
			}
			finally
			{
				RaiseListChangedEvents = previousValue;
				ResetBindings();
			}
		}

		public void ApplyFilter(Predicate<T> filter)
		{
			this.filter = filter;
			FilterAndSort();
		}

		#region IBindingListView Members
		public void ApplySort(ListSortDescriptionCollection sortCollection)
		{
			sortComparers = new PropertyComparerCollection<T>(sortCollection);
			FilterAndSort();
		}

		string IBindingListView.Filter
		{
			get { return string.Empty; }
			set { throw new NotSupportedException();}
		}

		void IBindingListView.RemoveFilter()
		{
			filter = null;
			FilterAndSort();
		}

		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return sortComparers.Sorts; }
		}

		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}

		bool IBindingListView.SupportsFiltering
		{
			get { return true; }
		}
		#endregion

		protected override void RemoveSortCore()
		{
			sortComparers = null;
		}

		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			ApplySort(new ListSortDescriptionCollection(new[] { new ListSortDescription(property, direction) }));
		}
	}
/*
	public class BindingListAce<T>: BindingList<T>, IBindingListView
	{
		PropertyComparerCollection<T> sortComparers;

		protected override bool IsSortedCore
		{
			get { return sortComparers != null; }
		}

		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get
			{
				return sortComparers == null
				       	? ListSortDirection.Ascending
				       	: sortComparers.PrimaryDirection;
			}
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return sortComparers == null ? null : sortComparers.PrimaryProperty; }
		}

		#region IBindingListView Members
		public void ApplySort(ListSortDescriptionCollection sortCollection)
		{
			var previousValue = RaiseListChangedEvents;
			RaiseListChangedEvents = false;

			try
			{
				var items = (List<T>) Items;
				var newComparer = new PropertyComparerCollection<T>(sortCollection);

				items.Sort(newComparer);

				sortComparers = newComparer;
			}
			finally
			{
				RaiseListChangedEvents = previousValue;
				ResetBindings();
			}
		}

		string IBindingListView.Filter
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		void IBindingListView.RemoveFilter()
		{
			throw new NotImplementedException();
		}

		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return sortComparers.Sorts; }
		}

		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}

		bool IBindingListView.SupportsFiltering
		{
			get { return false; }
		}
		#endregion

		protected override void RemoveSortCore()
		{
			sortComparers = null;
		}

		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			ApplySort(new ListSortDescriptionCollection(new[] { new ListSortDescription(property, direction) }));
		}
	}
*/

	[ComVisible(false)]
	public class PropertyComparerCollection<T>: IComparer<T>
	{
		readonly PropertyComparer<T>[] comparers;
		readonly ListSortDescriptionCollection sorts;

		public PropertyComparerCollection(ListSortDescriptionCollection sorts)
		{
			if (sorts == null) throw new ArgumentNullException("sorts");

			this.sorts = sorts;

			var list = new List<PropertyComparer<T>>();

			foreach (ListSortDescription item in sorts)
			{
				list.Add(new PropertyComparer<T>(item.PropertyDescriptor,
				                                 item.SortDirection == ListSortDirection.Descending));
			}

			comparers = list.ToArray();
		}

		public ListSortDescriptionCollection Sorts
		{
			get { return sorts; }
		}

		public PropertyDescriptor PrimaryProperty
		{
			get
			{
				return comparers.Length == 0
				       	? null
				       	: comparers[0].Property;
			}
		}

		public ListSortDirection PrimaryDirection
		{
			get
			{
				return comparers.Length == 0
				       	? ListSortDirection.Ascending
				       	: comparers[0].Descending
				       	  	? ListSortDirection.Descending
				       	  	: ListSortDirection.Ascending;
			}
		}

		#region IComparer<T> Members
		int IComparer<T>.Compare(T x, T y)
		{
			var result = 0;

			for (var i = 0; i < comparers.Length; i++)
			{
				result = comparers[i].Compare(x, y);
				if (result != 0) break;
			}

			return result;
		}
		#endregion
	}

	[ComVisible(false)]
	public class PropertyComparer<T>: IComparer<T>
	{
		readonly bool descending;
		readonly PropertyDescriptor property;

		public PropertyComparer(PropertyDescriptor property, bool descending)
		{
			if (property == null) throw new ArgumentNullException("property");

			this.descending = descending;
			this.property = property;
		}

		public bool Descending
		{
			get { return descending; }
		}

		public PropertyDescriptor Property
		{
			get { return property; }
		}

		#region IComparer<T> Members
		public int Compare(T x, T y)
		{
			var value = Comparer.Default.Compare(property.GetValue(x),
			                                     property.GetValue(y));
			return descending ? -value : value;
		}
		#endregion
	}
}