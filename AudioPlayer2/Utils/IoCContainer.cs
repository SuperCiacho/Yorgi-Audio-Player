using System;
using System.Collections.Generic;
using AudioPlayer2.Models.Audio;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Models.Tag;
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
            kernel.Bind<ICleanup>().To<MainViewModel>();
            kernel.Bind<IAudioPlayer>().To<AudioPlayer>();
            kernel.Bind<ITagManager>().To<TagManager>().InSingletonScope();
            kernel.Bind<IPlaylist>().To<Playlist>().InSingletonScope();
            kernel.Bind<ITaggedFile>().To<TaggedFile>();
            kernel.Bind<IDialogManager>().To<DialogManager>();
        }

        public T Get<T>(string name = null, IParameter[] parameters = null)
        {
            return (T)this.Get(typeof(T), name, parameters);
        }

        public object Get(Type type, string name = null, IParameter[] parameters = null)
        {
            if (name != null && parameters != null) return kernel.Get(type, name, parameters);

            if (parameters != null) return kernel.Get(type, parameters);

            return kernel.Get(type);
        }

        public T TryGet<T>(string name = null, IParameter[] parameters = null)
        {
            return (T)this.TryGet(typeof(T), name, parameters);
        }

        public object TryGet(Type type, string name = null, IParameter[] parameters = null)
        {
            if (name != null && parameters != null) kernel.TryGet(type, name, parameters);

            if (parameters != null) return kernel.TryGet(type, parameters);

            return kernel.TryGet(type);
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
