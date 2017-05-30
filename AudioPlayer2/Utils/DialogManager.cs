using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AudioPlayer2.Models;
using AudioPlayer2.Views;
using GalaSoft.MvvmLight;
using Ninject.Parameters;

namespace AudioPlayer2.Utils
{
    internal class DialogManager
    {
        public static Window CreateDialog<TView, TViewModel>(string caption, int height, int width, Dictionary<string, object> vmParameters = null) 
            where TView : FrameworkElement
            where TViewModel : ViewModelBase
        {
            var ioc = IoCContainer.Instance;

            IParameter[] viewModelConstructorParams = null;

            if (vmParameters != null)
            {
                viewModelConstructorParams = vmParameters.Select(x => IoCContainer.Instance.CreateConstructorArgument(x.Key, x.Value)).ToArray();
            }

            var dataContext = vmParameters != null ? ioc.Get<TViewModel>(parameters: viewModelConstructorParams) : ioc.Get<TViewModel>();
            var parameterName = typeof (TView).GetConstructors().SelectMany(c => c.GetParameters()).First(p => p.ParameterType == typeof (TViewModel)).Name;
            var content = ioc.Get<TView>(parameters: new[] {ioc.CreateConstructorArgument(parameterName, dataContext)});

            var shell = new Window
                        {
                            Content = content,
                            Title = caption,
                            Height = height,
                            Width = width,
                            WindowStyle = WindowStyle.ToolWindow
                        };

            return shell;

        }
    }
}
