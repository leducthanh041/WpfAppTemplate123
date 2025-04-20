using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WpfAppTemplate.Views;
using System.Windows.Input;
using System.Windows;

using WpfAppTemplate.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using WpfAppTemplate.Models;
using System.Collections.ObjectModel;

using WpfAppTemplate.Services;

namespace WpfAppTemplate.ViewModels
{
    public class TiepNhanDaiLyViewModel : INotifyPropertyChanged
    {

        public TiepNhanDaiLyViewModel(
            IQuanService quanService,
            IDaiLyService daiLyService,
            ILoaiDaiLyService loaiDaiLyService
            )
        {
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanDaiLyCommand = new RelayCommand(async () => await TiepNhanDaiLy());
            DaiLyMoiCommand = new RelayCommand(DaiLyMoi);

            _quanService = quanService;
            _loaiDaiLyService = loaiDaiLyService;
            _daiLyService = daiLyService;

            _ = LoadDataAsync();
        }

        // Properties for binding
        private string _maDaiLy = string.Empty;
        private string _tenDaiLy = string.Empty;
        private string _soDienThoai = string.Empty;
        private string _email = string.Empty;
        private DateTime _ngayTiepNhan = DateTime.Now;
        private string _diaChi = string.Empty;
        private LoaiDaiLy _selectedLoaiDaiLy = new();
        private Quan _selectedQuan = new();
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        private ObservableCollection<Quan> _quans = [];

        public string MaDaiLy
        {
            get => _maDaiLy;
            set
            {
                _maDaiLy = value;
                OnPropertyChanged();
            }
        }

        public string TenDaiLy
        {
            get => _tenDaiLy;
            set
            {
                _tenDaiLy = value;
                OnPropertyChanged();
            }
        }

        public string SoDienThoai
        {
            get => _soDienThoai;
            set
            {
                _soDienThoai = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public DateTime NgayTiepNhan
        {
            get => _ngayTiepNhan;
            set
            {
                _ngayTiepNhan = value;
                OnPropertyChanged();
            }
        }

        public string DiaChi
        {
            get => _diaChi;
            set
            {
                _diaChi = value;
                OnPropertyChanged();
            }
        }

        public LoaiDaiLy SelectedLoaiDaiLy
        {
            get => _selectedLoaiDaiLy;
            set
            {
                _selectedLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        public Quan SelectedQuan
        {
            get => _selectedQuan;
            set
            {
                _selectedQuan = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LoaiDaiLy> LoaiDaiLies
        {
            get => _loaiDaiLies;
            set
            {
                _loaiDaiLies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Quan> Quans
        {
            get => _quans;
            set
            {
                _quans = value;
                OnPropertyChanged();
            }
        }

        // Events
        public event EventHandler? DataChanged;

        // Services 
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IDaiLyService _daiLyService;

        // Command
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanDaiLyCommand { get; }
        public ICommand DaiLyMoiCommand { get; }

        private async Task LoadDataAsync()
        {
            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
            var listQuan = await _quanService.GetAllQuan();

            LoaiDaiLies.Clear();
            Quans.Clear();
            LoaiDaiLies = [.. listLoaiDaiLy];
            Quans = [.. listQuan];

            // Auto-select the first items in both ComboBoxes if available
            if (LoaiDaiLies.Count > 0)
            {
                SelectedLoaiDaiLy = LoaiDaiLies[0];
            }

            if (Quans.Count > 0)
            {
                SelectedQuan = Quans[0];
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<TiepNhanDaiLyWindow>().FirstOrDefault()?.Close();
        }

        private async Task TiepNhanDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenDaiLy))
            {
                MessageBox.Show("Tên đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy) || string.IsNullOrEmpty(SelectedQuan.TenQuan))
            {
                MessageBox.Show("Vui lòng chọn loại đại lý và quận!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(SoDienThoai))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(DiaChi))
            {
                MessageBox.Show("Địa chỉ không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MaDaiLy = (await _daiLyService.GenerateAvailableId()).ToString();
            DaiLy daiLy = new()
            {
                MaDaiLy = int.Parse(MaDaiLy),
                TenDaiLy = TenDaiLy,
                DienThoai = SoDienThoai,
                Email = Email,
                NgayTiepNhan = NgayTiepNhan,
                DiaChi = DiaChi,
                MaLoaiDaiLy = SelectedLoaiDaiLy.MaLoaiDaiLy,
                MaQuan = SelectedQuan.MaQuan,
                LoaiDaiLy = SelectedLoaiDaiLy,
                Quan = SelectedQuan
            };

            try
            {
                await _daiLyService.AddDaiLy(daiLy);
                MessageBox.Show("Tiếp nhận đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lưu đại lý không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DaiLyMoi()
        {
            MaDaiLy = string.Empty;
            TenDaiLy = string.Empty;
            SoDienThoai = string.Empty;
            Email = string.Empty;
            NgayTiepNhan = DateTime.Now;
            DiaChi = string.Empty;
            SelectedLoaiDaiLy = null!;
            SelectedQuan = null!;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
