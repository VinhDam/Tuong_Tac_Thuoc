using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tuong_Tac_Thuoc.Models;
using Tuong_Tac_Thuoc.Utilities;

namespace Tuong_Tac_Thuoc.Functions
{
    public class Export_Excel
    {
        public static void Export_KetQuaTuongTac_Excel(string path, List<KetQuaTuongTac> ketQuaTuongTacs)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Kết quả tương tác";
                p.Workbook.Worksheets.Add("KQ_Tuong_Tac");
                ExcelWorksheet ws = p.Workbook.Worksheets[0];
                ws.Name = "KQTT";
                ws.Cells.Style.Font.Size = 12;
                ws.Cells.Style.Font.Name = "times new roman";
                ws.Cells[1, 1].Value = "Mã bệnh nhân";
                ws.Cells[1, 2].Value = "Họ tên";
                ws.Cells[1, 3].Value = "Ngày";
                ws.Cells[1, 4].Value = "Hoạt chất";
                ws.Cells[1, 5].Value = "Kiểu tương tác";
                for (int i = 0; i < ketQuaTuongTacs.Count(); i++)
                {
                    ws.Cells[i+2, 1].Value = ketQuaTuongTacs[i].MaSo;
                    ws.Cells[i+2, 2].Value = ketQuaTuongTacs[i].HoTen;
                    ws.Cells[i+2, 3].Value = ketQuaTuongTacs[i].NgayThangA + " - "+ketQuaTuongTacs[i].NgayThangB;
                    ws.Cells[i+2, 4].Value = ketQuaTuongTacs[i].HoatChatA + " - "+ketQuaTuongTacs[i].HoatChatB;
                    ws.Cells[i+2, 5].Value = ketQuaTuongTacs[i].KieuTuongTac;
                }
                ws.View.FreezePanes(1, 7);
                ws.Cells["A1:G1"].AutoFilter = true;
                Byte[] bin = p.GetAsByteArray();
                File.WriteAllBytes(path, bin);
            }
        }
        public static void Export_Count_Excel(string path)
        {
            List<ThongTinBenhNhan> thongTinBenhNhans = new List<ThongTinBenhNhan>();
            string pathThongTinBenhNhan = @".\ThongTinBenhNhan.txt";
            FileStream openThongTinBenhNhan = new FileStream(pathThongTinBenhNhan, FileMode.Open);
            using (StreamReader readThongTinBenhNhan = new StreamReader(openThongTinBenhNhan))
            {
                string line;
                while ((line = readThongTinBenhNhan.ReadLine()) != null)
                {
                    string[] items = line.Split(',');
                    ThongTinBenhNhan thongTinBenhNhan = new ThongTinBenhNhan();
                    thongTinBenhNhan.MaSo = int.Parse(items[0]);
                    thongTinBenhNhan.HoTen = items[1];
                    thongTinBenhNhan.NgayThang = DateOnly.Parse(items[2]);
                    thongTinBenhNhan.Thuoc = items[3];
                    thongTinBenhNhans.Add(thongTinBenhNhan);
                }
            }
            List<ThongTinBenhNhan> querythongTinBenhNhans = thongTinBenhNhans.Distinct(new ThongTinBenhNhanComparer()).ToList();
            querythongTinBenhNhans = querythongTinBenhNhans.OrderBy(e => e.MaSo).ToList();

            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Kết quả tương tác";
                p.Workbook.Worksheets.Add("Count_Benh_Nhan");
                ExcelWorksheet ws = p.Workbook.Worksheets[0];
                ws.Name = "CBN";
                ws.Cells.Style.Font.Size = 12;
                ws.Cells.Style.Font.Name = "times new roman";
                ws.Cells[1, 1].Value = "Mã số";
                ws.Cells[1, 2].Value = "Họ tên";
                for (int i = 0; i < querythongTinBenhNhans.Count(); i++)
                {
                    ws.Cells[i+2, 1].Value = querythongTinBenhNhans[i].MaSo;
                    ws.Cells[i+2, 2].Value = querythongTinBenhNhans[i].HoTen;
                }
                ws.Cells[1, 3].Value = "Số lượng bệnh nhân: ";
                ws.Cells[1, 4].Value = querythongTinBenhNhans.Count();
                Byte[] bin = p.GetAsByteArray();
                File.WriteAllBytes(path, bin);
            }
        }
        public static void Export_FileGop_Excel(string path, string file)
        {
            List<string> DanhSachPath = new List<string>();
            string pathDanhSachPath = @".\DanhSachPath.txt";
            FileStream openDanhSachPath = new FileStream(pathDanhSachPath, FileMode.Open);
            using (StreamReader readDanhSachPath = new StreamReader(openDanhSachPath))
            {
                string line;
                while ((line = readDanhSachPath.ReadLine()) != null)
                {
                    DanhSachPath.Add(line);
                }
            }

            int i = 1;
            using (ExcelPackage NewFile = new ExcelPackage())
            {
                foreach (var filepath in DanhSachPath)
                {
                    if (filepath.Contains(file))
                    {
                        using (var secondFile = new ExcelPackage(filepath))
                        {
                            foreach (var sheet in secondFile.Workbook.Worksheets)
                            {
                                NewFile.Workbook.Worksheets.Add(file+" "+i.ToString(), sheet);
                                i++;
                            }
                        }
                    }
                }
                Byte[] bin = NewFile.GetAsByteArray();
                File.WriteAllBytes(path, bin);
            }
        }
    }
}
