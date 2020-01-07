using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneCSharp.MVVM
{
    public static class TaskUtilities
    {
        #pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
        #pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
    public interface IAsyncCommand : ICommand
    {
        bool CanExecute();
        Task ExecuteAsync(object parameter);
    }
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<bool> _canExecute;
        private readonly Func<object, Task> _execute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(Func<object, Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }
        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }
        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        #endregion
    }

    //public interface IAsyncCommand<T> : ICommand
    //{
    //    Task ExecuteAsync(T parameter);
    //    bool CanExecute(T parameter);
    //}
    //public class AsyncCommand<T> : IAsyncCommand<T>
    //{
    //    public event EventHandler CanExecuteChanged;

    //    private bool _isExecuting;
    //    private readonly Func<T, Task> _execute;
    //    private readonly Func<T, bool> _canExecute;
    //    private readonly IErrorHandler _errorHandler;

    //    public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
    //    {
    //        _execute = execute;
    //        _canExecute = canExecute;
    //        _errorHandler = errorHandler;
    //    }

    //    public bool CanExecute(T parameter)
    //    {
    //        return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    //    }

    //    public async Task ExecuteAsync(T parameter)
    //    {
    //        if (CanExecute(parameter))
    //        {
    //            try
    //            {
    //                _isExecuting = true;
    //                await _execute(parameter);
    //            }
    //            finally
    //            {
    //                _isExecuting = false;
    //            }
    //        }

    //        RaiseCanExecuteChanged();
    //    }

    //    public void RaiseCanExecuteChanged()
    //    {
    //        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    //    }

    //    #region Explicit implementations
    //    bool ICommand.CanExecute(object parameter)
    //    {
    //        return CanExecute((T)parameter);
    //    }

    //    void ICommand.Execute(object parameter)
    //    {
    //        ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
    //    }
    //    #endregion
    //}
}