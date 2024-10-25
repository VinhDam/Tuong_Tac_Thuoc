using Microsoft.Win32;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tuong_Tac_Thuoc.Functions;
using Tuong_Tac_Thuoc.Models;
using Tuong_Tac_Thuoc.Utilities;

namespace Tuong_Tac_Thuoc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_DuLieu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().HasValue)
            {
                try
                {
                    Import_Excel.Import_DuLieu_Excel(ofd.FileName, tb_DuLieu);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Nhập file không thành công");
                }
            }
        }
        private void btn_Kho_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter="Excel Files (*.xlsx)|*.xlsx|Xls Files (*.xls)|*.xls";
            if (ofd.ShowDialog().HasValue)
            {
                try
                {
                    Import_Excel.Import_TheKho_Excels(ofd.FileNames, lb_Kho);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Nhập file không thành công");
                }
            }
        }
        private void btn_CountThuoc_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Đếm bệnh nhân";
            dlg.Filter ="Excel (*.xlsx)|*.xlsx|Excel 2003 (*.xls)|*.xls";
            if (lb_Kho.Items.Count==0)
            {
                MessageBox.Show("Chưa chọn files thẻ kho");
                return;
            }
            if (dlg.ShowDialog().HasValue)
            {
                try
                {
                    Export_Excel.Export_Count_Excel(dlg.FileName);
                    MessageBox.Show("Xuất file thành công");
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Xuất file không thành công");
                }
            }
        }
        private void btn_KiemTraTuongTac_Click(object sender, RoutedEventArgs e)
        {
            List<KetQuaTuongTac> ketQuaTuongTacs = KiemTraTuongTac.CheckTuongTac(dtp_FromDate, dtp_ToDate);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Tương tác";
            dlg.Filter ="Excel (*.xlsx)|*.xlsx|Excel 2003 (*.xls)|*.xls";
            if (string.IsNullOrEmpty(tb_DuLieu.Text) && lb_Kho.Items.Count==0)
            {
                MessageBox.Show("Chưa chọn file");
                return;
            }
            if (dlg.ShowDialog().HasValue)
            {
                try
                {
                    Export_Excel.Export_KetQuaTuongTac_Excel(dlg.FileName, ketQuaTuongTacs);
                    MessageBox.Show("Xuất file thành công");
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Xuất file không thành công");
                }
            }
        }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btn_FileCanGop_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter="Excel Files (*.xlsx)|*.xlsx|Xls Files (*.xls)|*.xls";
            if (ofd.ShowDialog().HasValue)
            {
                try
                {
                    Import_Excel.Import_FileGop_Excels(ofd.FileNames, lb_FileGop);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Nhập file không thành công");
                }
            }
        }
        private void btn_GopFiles_Click(object sender, RoutedEventArgs e)
        {
            if (lb_FileGop.Items.Count==0)
            {
                MessageBox.Show("Chưa chọn file");
                return;
            }

            List<string> DanhSachGop = new List<string>();
            string pathDanhSachGop = @".\DanhSachGop.txt";
            FileStream openDanhSachGop = new FileStream(pathDanhSachGop, FileMode.Open);
            using (StreamReader readDanhSachGop = new StreamReader(openDanhSachGop))
            {
                string line;
                while ((line = readDanhSachGop.ReadLine()) != null)
                {
                    DanhSachGop.Add(line);
                }
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Tương tác";
            dlg.Filter ="Excel (*.xlsx)|*.xlsx|Excel 2003 (*.xls)|*.xls";
            if (dlg.ShowDialog().HasValue)
            {
                try
                {
                    foreach(var file in DanhSachGop)
                    {
                        dlg.FileName = dlg.FileName.Replace(dlg.SafeFileName, file+".xlsx");
                        Export_Excel.Export_FileGop_Excel(dlg.FileName, file);
                    }
                    MessageBox.Show("Xuất file thành công");
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Xuất file không thành công");
                }
            }
        }
    }
}