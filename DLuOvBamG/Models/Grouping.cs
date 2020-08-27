using DLToolkit.Forms.Controls;
using System.Collections.Generic;

namespace DLuOvBamG.Models
{
	public class Grouping<K, T> : FlowObservableCollection<T>
	{
		public K Key { get; private set; }
		public int ColumnCount { get; set; }

		public IEnumerable<T> GroupedItems {
			get
			{
				return Items;
			}

			private set { }
		}
		public Grouping(K key)
		{
			Key = key;
		}

		public Grouping(K key, IEnumerable<T> items)
			: this(key)
		{
			AddRange(items);
		}

		public Grouping(K key, IEnumerable<T> items, int columnCount)
			: this(key, items)
		{
			ColumnCount = columnCount;
		}

    }
}
