using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppTemplate.Views;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Runtime.CompilerServices;
using WpfAppTemplate.Services;
using System.Collections.ObjectModel;
using WpfAppTemplate.Models;
using WpfAppTemplate.Commands;

namespace WpfAppTemplate.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _daiLyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatDaiLyViewModel> _capNhatDaiLyFactory;

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            IDaiLyService daiLyService,
            Func<int, CapNhatDaiLyViewModel> capNhatDaiLyFactory
        )
        {
            _daiLyService = daiLyService;
            _ = LoadData();

            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());
            TiepNhanDaiLyCommand = new RelayCommand(OpenTiepNhanDaiLyWindow);
            EditDaiLyCommand = new RelayCommand(OpenCapNhatDaiLyWindow);

            _serviceProvider = serviceProvider;
            _capNhatDaiLyFactory = capNhatDaiLyFactory;
        }

        private ObservableCollection<DaiLy> _danhSachDaiLy = [];
        public ObservableCollection<DaiLy> DanhSachDaiLy
        {
            get => _danhSachDaiLy;
            set
            {
                _danhSachDaiLy = value;
                OnPropertyChanged();
            }
        }

        private DaiLy _selectedDaiLy = new();
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadData()
        {
            var list = await _daiLyService.GetAllDaiLy();
            DanhSachDaiLy = [.. list];
            SelectedDaiLy = null!;
        }

        public ICommand LoadDataCommand { get; }
        public ICommand TiepNhanDaiLyCommand { get; }
        public ICommand EditDaiLyCommand { get; }

        private async Task LoadDataExecute()
        {
            await LoadData();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenTiepNhanDaiLyWindow()
        {
            SelectedDaiLy = null!;

            var tiepNhanDaiLyWindow = _serviceProvider.GetRequiredService<TiepNhanDaiLyWindow>();

            if (tiepNhanDaiLyWindow.DataContext is TiepNhanDaiLyViewModel viewModel)
            {
                viewModel.DataChanged += async (sender, e) => await LoadData();
            }

            tiepNhanDaiLyWindow.Show();
        }

        private void OpenCapNhatDaiLyWindow()
        {
            if (SelectedDaiLy == null || string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewModel = _capNhatDaiLyFactory(SelectedDaiLy.MaDaiLy);
                viewModel.DataChanged += async (sender, e) => await LoadData();

                var window = new CapNhatDaiLyWindow(viewModel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
