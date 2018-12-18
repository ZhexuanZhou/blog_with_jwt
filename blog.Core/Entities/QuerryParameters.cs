using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using blog.Core.Interfaces;

namespace blog.Core.Entities
{
    public abstract class QuerryParameters : INotifyPropertyChanged
    {
        private const int DefaultPageSize = 10;
        private const int DefaultMaxPageSize = 100;

        private int _pageIndex;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value > 0 ? value : 0;
        }

        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => SetField(ref _pageSize, value);
        }

        private string _orderBy;
        public string Orderby
        {
            get => _orderBy;
            set => _orderBy = value ?? nameof(IEntity.Id);
        }

        private int _maxPageSize = DefaultMaxPageSize;
        public int MaxPageSize
        {
            get => _maxPageSize;
            set => SetField(ref _maxPageSize, value);
        }

        public string Fields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //[CallerMemberName] 获取调用方的函数名称和位置
            // PropertyChanged不为空的时候执行invoke
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // if(PropertyChanged!=null)
            // {
            //     PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // }
        }

        // 更新Property
        protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            if (propertyName == nameof(PageSize) || propertyName == nameof(MaxPageSize))
            {
                SetPageSize();
            }
            return true;
        }

        private void SetPageSize()
        {
            if (_maxPageSize <= 0)
            {
                _maxPageSize = DefaultMaxPageSize;
            }
            if (_pageSize <= 0)
            {
                _pageSize = DefaultPageSize;
            }
            _pageSize = _pageSize > _maxPageSize ? _maxPageSize : _pageSize;
        }
    }
}
