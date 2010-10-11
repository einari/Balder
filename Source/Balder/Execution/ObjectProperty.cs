﻿using System;
using Balder.Rendering;

#if(SILVERLIGHT)
using System.ComponentModel;
using System.Windows;
#endif

namespace Balder.Execution
{
    public class ObjectProperty<T>
    {
    	private readonly PropertyDescriptor _propertyDescriptor;
    	private readonly PropertyValueChanged<T> _propertyValueChanged;

    	public WeakReference Object;
        public bool CallFromExternal;
        public bool CallFromProperty;
    	public IRuntimeContext RuntimeContext;
    	public bool ChildrenRuntimeContextSet;

		private T _value;
    	private T _oldValue;

#if(SILVERLIGHT)
        internal ObjectProperty(DependencyObject obj, PropertyDescriptor propertyDescriptor, T defaultValue, PropertyValueChanged<T> propertyValueChanged)
#else
		internal ObjectProperty(object obj, PropertyDescriptor propertyDescriptor, T defaultValue)
#endif
		{
        	_propertyDescriptor = propertyDescriptor;
        	_propertyValueChanged = propertyValueChanged;
        	Value = defaultValue;
            Object = new WeakReference(obj);
            CallFromExternal = false;
            CallFromProperty = false;
			ChildrenRuntimeContextSet = false;
        }

    	
    	public T Value
    	{
    		get { return _value; }
    		set
    		{
    			_oldValue = _value;
    			_value = value;
				HandleValueSet();
    		}
    	}

		private void HandleValueSet()
		{
#if(SILVERLIGHT)
			if (_propertyDescriptor.IsValueNotifyPropertyChanged)
			{
				if (null != _oldValue)
				{
					((INotifyPropertyChanged)_oldValue).PropertyChanged -= HandleNotification;
				}
				if (null != _value)
				{
					((INotifyPropertyChanged)_value).PropertyChanged += HandleNotification;
				}
			}
#endif
		}

#if(SILVERLIGHT)
		private void HandleNotification(object obj, PropertyChangedEventArgs e)
		{
			if (null != _propertyValueChanged)
			{
				_propertyValueChanged(Object.Target, _oldValue, _value);
			}
		}
#endif

    	public bool DoesValueCauseChange(T value)
        {
            if (!_propertyDescriptor.IsValueType)
            {
                if ((null == (object)Value && null != (object)value) ||
                    null == (object)value || !value.Equals(Value))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return !value.Equals(Value);
            }
        }

		public void SignalRendering()
		{
			if( null != RuntimeContext )
			{
				RuntimeContext.SignalRendering();
			}
		}
    }
}