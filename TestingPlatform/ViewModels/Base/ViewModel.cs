using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestingPlatform.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            // CallerMemberName - компилятор автоматом подставит имя метода из которого вызывается метод
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Обновление значения свойства (для удобства)
        /// Изменяет свойство по ссылке если value не равно field (не равно старому значению)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Ссылка на поле свойства</param>
        /// <param name="value">Новое значение</param>
        /// <param name="">Имя свойства</param>
        /// <returns>Возвращает true если новое value не равно field, иначе false</returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed) return;
            _disposed = true;
            // освобождение управляемых ресурсов
        }
    }
}
