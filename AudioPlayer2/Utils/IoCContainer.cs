using System.Collections.Generic;
using AudioPlayer2.Models;
using AudioPlayer2.ViewModels;
using GalaSoft.MvvmLight;
using Ninject;
using Ninject.Parameters;

namespace AudioPlayer2.Utils
{
    internal sealed class IoCContainer
    {
        private static readonly IKernel kernel;

        public static IoCContainer Instance { get; } = new IoCContainer();

        static IoCContainer()
        {
            kernel = new StandardKernel();
            kernel.Bind<IAudioPlayer>().To<AudioPlayer>();
            kernel.Bind<ICleanup>().To<MainViewModel>();
            kernel.Bind<ITagManager>().To<TagManager>().InSingletonScope();
            kernel.Bind<ITaggedFile>().To<TaggedFile>();
        }

        public T Get<T>(string name = null, IParameter[] parameters = null)
        {
            if (name != null && parameters != null) return kernel.Get<T>(name, parameters);

            if (parameters != null) return kernel.Get<T>(parameters);

            return kernel.Get<T>();
        }

        public T TryGet<T>(string name = null, IParameter[] parameters = null)
        {
            if (name != null && parameters != null) kernel.TryGet<T>(name, parameters);

            if (parameters != null) return kernel.TryGet<T>(parameters);

            return kernel.TryGet<T>();
        }

        public IEnumerable<T> GetAll<T>(string name = null, IParameter[] parameters = null)
        {
            if (name != null && parameters != null) kernel.GetAll<T>(name, parameters);

            if (parameters != null) return kernel.GetAll<T>(parameters);

            return kernel.GetAll<T>();
        }

        public IParameter CreateConstructorArgument(string argumentName, object argumentValue)
        {
            return new ConstructorArgument(argumentName, argumentValue);
        }
    }
}
