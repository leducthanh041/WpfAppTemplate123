using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfAppTemplate.Models;
using WpfAppTemplate.Services;
using WpfAppTemplate.Commands;
using WpfAppTemplate.Views;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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
            DeleteDaiLyCommand = new RelayCommand(OpenXoaDaiLyWinDow);


            _serviceProvider = serviceProvider;
            _capNhatDaiLyFactory = capNhatDaiLyFactory;
        }

        // Properties for binding
        private ObservableCollection<DaiLy> _danhSachDaiLy = [];
        private DaiLy _selectedDaiLy = new();

        public ObservableCollection<DaiLy> DanhSachDaiLy
        {
            get => _danhSachDaiLy;
            set
            {
                _danhSachDaiLy = value;
                OnPropertyChanged();
            }
        }

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
        public ICommand DeleteDaiLyCommand { get; }

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

        private async void OpenXoaDaiLyWinDow()
        {
            if (string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đại lý '{SelectedDaiLy.TenDaiLy}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _daiLyService.DeleteDaiLy(SelectedDaiLy.MaDaiLy);
                    await LoadData();
                    MessageBox.Show("Đã xóa đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
