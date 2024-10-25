using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Tuong_Tac_Thuoc.Models;
using Tuong_Tac_Thuoc.Utilities;

namespace Tuong_Tac_Thuoc.Functions
{
    public class Import_Excel
    {
        public static void Import_DuLieu_Excel(string fileName, TextBox tb_DuLieu)
        {
            string pathDuLieuTuongTac = @".\DuLieuTuongTac.txt";
            if (File.Exists(pathDuLieuTuongTac))
            {
                File.Delete(pathDuLieuTuongTac);
            }
            FileStream openDuLieuTuongTac = new FileStream(pathDuLieuTuongTac, FileMode.Create);
            StreamWriter createDuLieuTuongTac = new StreamWriter(openDuLieuTuongTac);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach(var item in package.Workbook.Worksheets)
                {
                    ExcelWorksheet ws = item;
                    tb_DuLieu.Text = package.File.Name;
                    for (int i = 2; i<=ws.Dimension.End.Row; i++)
                    {
                        DuLieuTuongTac duLieuTuongTac = new DuLieuTuongTac();
                        if (ws.Cells[i, 2].Value!=null &&
                           ws.Cells[i, 3].Value!=null &&
                           ws.Cells[i, 7].Value!=null)
                        {
                            createDuLieuTuongTac.WriteLine(ws.Cells[i, 2].Text+","+ws.Cells[i, 3].Text+","+ws.Cells[i, 7].Text);
                        }
                    }
                }
            }
            createDuLieuTuongTac.Close();
            openDuLieuTuongTac.Close();
        }
        public static void Import_TheKho_Excels(string[] fileNames, ListBox lb_Kho)
        {
            string pathThongTinBenhNhan = @".\ThongTinBenhNhan.txt";
            if (File.Exists(pathThongTinBenhNhan))
            {
                File.Delete(pathThongTinBenhNhan);
            }
            FileStream openThongTinBenhNhan = new FileStream(pathThongTinBenhNhan, FileMode.Create);
            StreamWriter createThongTinBenhNhan = new StreamWriter(openThongTinBenhNhan);

            string pathDanhSachThuoc = @".\DanhSachThuoc.txt";
            if (File.Exists(pathDanhSachThuoc))
            {
                File.Delete(pathDanhSachThuoc);
            }
            FileStream openDanhSachThuoc = new FileStream(pathDanhSachThuoc, FileMode.Create);
            StreamWriter createDanhSachThuoc = new StreamWriter(openDanhSachThuoc);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            foreach (string file in fileNames)
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(file.Contains("xlsx") ? file : XLS_To_XLSX.Convert_XLS_XLSX(new FileInfo(file)))))
                {
                    lb_Kho.Items.Add(package.File.Name);
                    string thuoc = package.File.Name.Replace(".xlsx", "");
                    createDanhSachThuoc.WriteLine(thuoc);

                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        ExcelWorksheet ws = sheet;
                        for (int i = 4; i<=ws.Dimension.End.Row; i++)
                        {
                            if (ws.Cells[i, 6].Value!=null && ws.Cells[i, 6].Value.ToString().Contains("Xuất cho bệnh nhân"))
                            {
                                bool x = int.TryParse(ws.Cells[i, 1].Value.ToString(), out int y);
                                string NgayThang = x==true ? DateOnly.FromDateTime(DateTime.FromOADate(y)).ToString() : DateOnly.Parse(ws.Cells[i, 1].Value.ToString()).ToString();
                                createThongTinBenhNhan.WriteLine(ws.Cells[i, 6].Value.ToString().Substring(19, 8)+","
                                           +ws.Cells[i, 6].Value.ToString().Substring(28)+","
                                           +NgayThang+","
                                           +thuoc);
                            }
                        }
                    }
                }
            }
            createThongTinBenhNhan.Close();
            openThongTinBenhNhan.Close();
            createDanhSachThuoc.Close();
            openDanhSachThuoc.Close();
        }
        public static void Import_FileGop_Excels(string[] fileNames, ListBox lb_FileGop)
        {
            string pathDanhSachGop = @".\DanhSachGop.txt";
            if (File.Exists(pathDanhSachGop))
            {
                File.Delete(pathDanhSachGop);
            }
            FileStream openDanhSachGop = new FileStream(pathDanhSachGop, FileMode.Create);
            StreamWriter createDanhSachGop = new StreamWriter(openDanhSachGop);

            string pathDanhSachPath = @".\DanhSachPath.txt";
            if (File.Exists(pathDanhSachPath))
            {
                File.Delete(pathDanhSachPath);
            }
            FileStream openDanhSachPath = new FileStream(pathDanhSachPath, FileMode.Create);
            StreamWriter createDanhSachPath = new StreamWriter(openDanhSachPath);

            List<string> DanhSachGop = new List<string>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            foreach (string file in fileNames)
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(file.Contains("xlsx") ? file : XLS_To_XLSX.Convert_XLS_XLSX(new FileInfo(file)))))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
                    lb_FileGop.Items.Add(package.File.Name);

                    string name = package.File.Name.Substring(0, package.File.Name.Length - 7);
                    DanhSachGop.Add(name);
                }
            }

            DanhSachGop = DanhSachGop.Distinct().ToList();

            foreach (var item in DanhSachGop)
            {
                createDanhSachGop.WriteLine(item);
            }
            foreach(var path in fileNames)
            {
                createDanhSachPath.WriteLine(path);
            }
            createDanhSachPath.Close();
            openDanhSachPath.Close();
            createDanhSachGop.Close();
            openDanhSachGop.Close();
        }
    }
}
