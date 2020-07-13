using DLToolkit.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DLuOvBamG.Models
{
	public class Grouping<K, T> : FlowObservableCollection<T>
	{
		public K Key { get; private set; }
		public int ColumnCount { get; set; }

		public IEnumerable<T> GroupedItems {
			get
			{
				return this.Items;
			}

			private set { 
			}
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
