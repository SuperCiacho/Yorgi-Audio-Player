using System;

namespace AudioPlayer2.Models
{
    internal interface IView
    {
        Type ViewModelType { get;  }
    }
}