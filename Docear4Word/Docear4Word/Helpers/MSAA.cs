using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

using Office;

using Word;

namespace Docear4Word
{
	public static class MSAA
	{
		public static bool IsReferencesTabActive(_Application application)
		{
			var ribbon = application.CommandBars["Ribbon"];
			var accessibleRibbon = ribbon as IAccessible;
		}

        public static List<MSAAUIItem> GetAllUIItemsOfType(AccessibleUIItemType uiItemType, bool ignoreInvisible)
        {
            List<MSAAUIItem> accUiItemList = new List<MSAAUIItem>();

            var accObjectList = new List<IAccessible>();

            MSAA.GetAccessibleObjectListByRole(_me, MSAARoles.GetRoleText(uiItemType), ref accObjectList, ignoreInvisible);

            foreach (IAccessible accUIObject in accObjectList)
            {
                MSAAUIItem accUIItem = new MSAAUIItem(accUIObject);

                if (accUIItem.Properties.Role == MSAARoles.GetRoleText(uiItemType))
                {
                    accUiItemList.Add(accUIItem);
                }
            }

            return accUiItemList;
        }

	}

    public class MSAAUIItem
    {
        #region Private Members

        IAccessible _me = default(IAccessible);
        IAccessible _parent = default(IAccessible);
        MSAAPropertySet _propertySet = null;
        int searchCycles = 15;
        int searchDuration = 2000;
        
        #endregion

        #region Public Properties

        public MSAAPropertySet Properties
        {
            get
            {
                _propertySet.Refresh();
                return _propertySet;
            }
        }

        public IAccessible Accessible
        {
            get { return _me; }
        }

        public IAccessible AccessibleParent
        {
            get { return _parent; }
        }

        #endregion
        
        #region Constructors

        public MSAAUIItem(Regex name)
        {
            for (int searchCycleCount = 0; searchCycleCount < searchCycles; searchCycleCount++)
            {
                _me = MSAA.GetTopWindowAccessibleObject(name);

                if (_me == null || _me == default(IAccessible))
                {
                    Thread.Sleep(searchDuration);
                }
                else
                {
                    _propertySet = new MSAAPropertySet(_me);
                    break;
                }
            }
        }

        public MSAAUIItem(string className, string caption)
        {
            for (int searchCycleCount = 0; searchCycleCount < searchCycles; searchCycleCount++)
            {
                _me = MSAA.GetTopWindowAccessibleObject(className, caption);

                if (_me == null || _me == default(IAccessible))
                {
                    Thread.Sleep(searchDuration);
                }
                else
                {
                    _propertySet = new MSAAPropertySet(_me);
                    break;
                }
            }
        }

        public MSAAUIItem(IAccessible parentAccObject, Regex name, bool ignoreInvisible)
        {
            for (int searchCycleCount = 0; searchCycleCount < searchCycles; searchCycleCount++)
            {
                _me = MSAA.GetObjectByName(parentAccObject, name, ignoreInvisible);
                _parent = parentAccObject;
                
                if (_me == null || _me == default(IAccessible))
                {
                    Thread.Sleep(searchDuration);
                }
                else
                {
                    _propertySet = new MSAAPropertySet(_me);
                    break;
                }
            }
            
        }

        public MSAAUIItem(IAccessible parentAccObject, Regex name, AccessibleUIItemType uiItemType, bool ignoreInvisible)
        {
            for (int searchCycleCount = 0; searchCycleCount < searchCycles; searchCycleCount++)
            {
                _me = MSAA.GetObjectByNameAndRole(parentAccObject, name, MSAARoles.GetRoleText(uiItemType), ignoreInvisible);
                _parent = parentAccObject;

                if (_me == null || _me == default(IAccessible))
                {
                    Thread.Sleep(searchDuration);
                }
                else
                {
                    _propertySet = new MSAAPropertySet(_me);
                    break;
                }
            }
            
            _propertySet = new MSAAPropertySet(_me);
        }

        public MSAAUIItem(IAccessible accObject)
        {
            _me = accObject;
            _propertySet = new MSAAPropertySet(_me);
        }


        #endregion

        #region Public Methods

        public List<MSAAUIItem> GetChildren()
        {
            List<MSAAUIItem> accUiItemList = new List<MSAAUIItem>();
            
            foreach(IAccessible accUIObject in MSAA.GetAccessibleChildren(_me))
            {
                MSAAUIItem accUIItem = new MSAAUIItem(accUIObject);
                accUiItemList.Add(accUIItem);
            }

            return accUiItemList;
        }

        public List<MSAAUIItem> GetChildren(AccessibleUIItemType uiItemType)
        {
            List<MSAAUIItem> accUiItemList = new List<MSAAUIItem>();

            foreach (IAccessible accUIObject in MSAA.GetAccessibleChildren(_me))
            {
                MSAAUIItem accUIItem = new MSAAUIItem(accUIObject);
                
                if(accUIItem.Properties.Role ==  MSAARoles.GetRoleText(uiItemType))
                {
                    accUiItemList.Add(accUIItem);
                }
            }

            return accUiItemList;
        }

        public List<MSAAUIItem> GetAllUIItemsOfType(AccessibleUIItemType uiItemType, bool ignoreInvisible)
        {
            List<MSAAUIItem> accUiItemList = new List<MSAAUIItem>();

            List<IAccessible> accObjectList = new List<IAccessible>();

            MSAA.GetAccessibleObjectListByRole(_me, MSAARoles.GetRoleText(uiItemType), ref accObjectList, ignoreInvisible);

            foreach (IAccessible accUIObject in accObjectList)
            {
                MSAAUIItem accUIItem = new MSAAUIItem(accUIObject);

                if (accUIItem.Properties.Role == MSAARoles.GetRoleText(uiItemType))
                {
                    accUiItemList.Add(accUIItem);
                }
            }

            return accUiItemList;
        }

        public bool Invoke()
        {
            bool defaultActionSuccess = false;

            if (_me != null && _me != default(IAccessible))
            {
                try
                {
                    _me.accDoDefaultAction(0);
                    defaultActionSuccess = true;
                }
                catch (Exception ex)
                {
                    defaultActionSuccess = false;
                }
            }
            return defaultActionSuccess;
        }

        #endregion

        #region Private Methods



        #endregion
    }

    internal class MSAARoles
    {
        public const string Client = "client";
        public const string ComboBox = "combo box";
        public const string Dialog = "dialog";
        public const string EditableText = "editable text";
        public const string GridDropDownButton = "grid drop down button";
        public const string List = "list";
        public const string ListItem = "list item";
        public const string MenuBar = "menu bar";
        public const string MenuItem = "menu item";
        public const string PageTab = "page tab";
        public const string Pane = "pane";
        public const string PropertyPage = "property page";
        public const string PushButton = "push button";
        public const string SplitButton = "split button";
        public const string ToolBar = "tool bar";
        public const string DropDown = "drop down";
        public const string Window = "window";
        public const string TitleBar = "title bar";
        public const string ScrolBar = "scroll bar";
        public const string Grip = "grip";

        public static string GetRoleText(AccessibleUIItemType role)
        {
            switch (role)
            {
                case AccessibleUIItemType.Client:
                    return Client;
                    break;
                case AccessibleUIItemType.ComboBox:
                    return ComboBox;
                    break;
                case AccessibleUIItemType.Dialog:
                    return Dialog;
                    break;
                case AccessibleUIItemType.DropDown:
                    return DropDown;
                    break;
                case AccessibleUIItemType.EditableText:
                    return EditableText;
                    break;
                case AccessibleUIItemType.GridDropDownButton:
                    return GridDropDownButton;
                    break;
                case AccessibleUIItemType.List:
                    return List;
                    break;
                case AccessibleUIItemType.ListItem:
                    return ListItem;
                    break;
                case AccessibleUIItemType.MenuBar:
                    return MenuBar;
                    break;
                case AccessibleUIItemType.MenuItem:
                    return MenuItem;
                    break;
                case AccessibleUIItemType.PageTab:
                    return PageTab;
                    break;
                case AccessibleUIItemType.Pane:
                    return Pane;
                    break;
                case AccessibleUIItemType.PropertyPage:
                    return PropertyPage;
                    break;
                case AccessibleUIItemType.PushButton:
                    return PushButton;
                    break;
                case AccessibleUIItemType.SplitButton:
                    return SplitButton;
                    break;
                case AccessibleUIItemType.TitleBar:
                    return TitleBar;
                    break;
                case AccessibleUIItemType.ToolBar:
                    return ToolBar;
                    break;
                case AccessibleUIItemType.Window:
                    return Window;
                    break;
                case AccessibleUIItemType.ScrollBar:
                    return ScrolBar;
                    break;
                case AccessibleUIItemType.Grip:
                    return Grip;
                    break;
                default:
                    return "";


            }
        }
    }

    public enum AccessibleUIItemType
    {
        Client,
        ComboBox,
        Dialog,
        EditableText,
        GridDropDownButton,
        List,
        ListItem,
        MenuBar,
        MenuItem,
        PageTab,
        Pane,
        PropertyPage,
        PushButton,
        SplitButton,
        ToolBar,
        DropDown,
        Window,
        TitleBar,
        ScrollBar,
        Grip
    }

    public class MSAAPropertySet
    {
        IAccessible _accessible = default(IAccessible);
        string _name = string.Empty;
        string _state = string.Empty;
        string _role = string.Empty;
        string _value = string.Empty;
        string _defaultAction = string.Empty;
        Rectangle _location = new Rectangle();
        IntPtr _handle = IntPtr.Zero;


        public string Name
        {
            get{return _name;}
        }

        public string State
        {
            get { return _state; }
        }

        public bool IsEnabled
        {
            get { return !State.Contains("unavailable"); }
        }

        public bool IsVisible
        {
            get { return !State.Contains("invisible"); }
        }

        public bool IsExist
        {
            get { return Handle != IntPtr.Zero; }
        }

        public string Role
        {
            get { return _role; }
        }

        public string Value
        {
            get { return _value; }
        }

        public Rectangle Location
        {
            get { return _location; }
        }

        public IntPtr Handle
        {
            get { return _handle; }
        }

        public string DefaultAction
        {
            get { return _defaultAction; }
        }

        public MSAAPropertySet(IAccessible accessibleObject)
        {
            _accessible = accessibleObject;
            
            if(_accessible != null && _accessible != default(IAccessible))
            {
                SetAccessibleProperties();
            }
        }

        public void Refresh()
        {
            if (_accessible != null && _accessible != default(IAccessible))
            {
                SetAccessibleProperties();
            }
        }

        private void SetAccessibleProperties()
        {
            //Here we are consuming the COM Exceptions which happens in case 
            //the property/Method we need is not available with IAccessible Object.

            try
            {
                _name = _accessible.get_accName(0);
            }
            catch (Exception ex)
            {
            }

            try
            {
                _value = _accessible.get_accValue(0);
            }
            catch (Exception ex)
            {
            }

            try
            {
                uint stateId = Convert.ToUInt32(_accessible.get_accState(0));
                _state = MSAA.GetStateText(stateId);
            }
            catch (Exception ex)
            {
            }

            try
            {
                uint roleId = Convert.ToUInt32(_accessible.get_accRole(0));
                _role = MSAA.GetRoleText(roleId);
            }
            catch (Exception ex)
            {
            }


            _handle = MSAA.GetHandle(_accessible);

            try
            {
                _defaultAction = _accessible.get_accDefaultAction(0);
            }
            catch (Exception ex)
            {
            }

            SetLocation(_accessible);
        }

        private void SetLocation(IAccessible accObject)
        {
            if (accObject != null)
            {
                int x1, y1;
                int width;
                int hieght;

                accObject.accLocation(out x1, out y1, out width, out hieght, 0);
                _location = new Rectangle(x1, y1, x1 + width, y1 + hieght);
            }
        }
    }

}