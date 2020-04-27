using OneCSharp.Integrator.Model;
using OneCSharp.MVVM;
using System;

namespace OneCSharp.Integrator.Module
{
    public sealed class WebServerViewModel : ViewModelBase
    {
        private string _textJSON = string.Empty;
        public WebServerViewModel(WebServerSettings model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
        public WebServerSettings Model { get; }
        public string TextJSON
        {
            get { return _textJSON; }
            set { _textJSON = value; OnPropertyChanged(nameof(TextJSON)); }
        }
    }
}