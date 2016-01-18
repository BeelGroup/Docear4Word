using System;

namespace Docear4Word
{
	public class JSOwnedItemWrapper<T> where T: JSObjectWrapper
	{
		readonly JSObjectWrapper owner;
		readonly string propertyName;
		T ownedItem;

		public JSOwnedItemWrapper(JSObjectWrapper owner, string propertyName)
		{
			this.owner = owner;
			this.propertyName = propertyName;
		}

		public T OwnedItem
		{
			get
			{
				if (ownedItem == null)
				{
					ownedItem = owner.Context.CreateWrappedJSObject<T>();
					ConfigureOwnedItem();
				}

				return ownedItem;
			}
			set
			{
/*
				if (ownedItem != null)
				{
					ownedItem.Ownerxxx = null;
				}
*/

				ownedItem = value;
/*

				if (value != null)
				{
					ConfigureOwnedItem();
					owner.NotifyOwnerToAddProperty(ownedItem);
				}
*/
			}
		}

		void ConfigureOwnedItem()
		{
			ownedItem.Context = owner.Context;
/*
			ownedItem.Ownerxxx = new WrapperOwnerInfo
			                  	{
			                  		Owner = owner,
			                  		PropertyName = propertyName
			                  	};
*/
		}
	}
}