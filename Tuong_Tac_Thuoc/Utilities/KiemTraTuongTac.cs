using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tuong_Tac_Thuoc.Models;

namespace Tuong_Tac_Thuoc.Utilities
{
    public class KiemTraTuongTac
    {
        public static List<KetQuaTuongTac> CheckTuongTac(DatePicker FromDate,DatePicker ToDate)
        {
            List<DuLieuTuongTac> duLieuTuongTacs = new List<DuLieuTuongTac>();
            string pathDuLieuTuongTac = @".\DuLieuTuongTac.txt";
            FileStream openDuLieuTuongTac = new FileStream(pathDuLieuTuongTac, FileMode.Open);
            using (StreamReader readDuLieuTuongTac = new StreamReader(openDuLieuTuongTac))
            {
                string line;
                while ((line = readDuLieuTuongTac.ReadLine()) != null)
                {
                    string[] items = line.Split(',');
                    DuLieuTuongTac duLieuTuongTac = new DuLieuTuongTac();
                    duLieuTuongTac.HoatChatA = items[0];
                    duLieuTuongTac.HoatChatB = items[1];
                    duLieuTuongTac.PhanLoai = items[2];
                    duLieuTuongTacs.Add(duLieuTuongTac);
                }
            }

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
            thongTinBenhNhans = thongTinBenhNhans.Distinct(new TuongTacComparer()).ToList();

            List<KetQuaTuongTac> ketQuaTuongTacs = new List<KetQuaTuongTac>();
            for (int i = 0; i< thongTinBenhNhans.Count; i++)
            {
                for (int j = 0; j<thongTinBenhNhans.Count; j++)
                {
                    if (thongTinBenhNhans[j].MaSo ==thongTinBenhNhans[i].MaSo)
                    {
                        KetQuaTuongTac ketQuaTuongTac = new KetQuaTuongTac();
                        ketQuaTuongTac.MaSo = thongTinBenhNhans[i].MaSo;
                        ketQuaTuongTac.HoTen = thongTinBenhNhans[i].HoTen;
                        ketQuaTuongTac.NgayThangA = thongTinBenhNhans[i].NgayThang.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        ketQuaTuongTac.NgayThangB = thongTinBenhNhans[j].NgayThang.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        ketQuaTuongTac.HoatChatA = thongTinBenhNhans[i].Thuoc;
                        ketQuaTuongTac.HoatChatB = thongTinBenhNhans[j].Thuoc;
                        foreach (var item in duLieuTuongTacs)
                        {
                            if (ketQuaTuongTac.HoatChatA == item.HoatChatA && ketQuaTuongTac.HoatChatB == item.HoatChatB)
                            {
                                ketQuaTuongTac.KieuTuongTac = item.PhanLoai;
                            }
                        }
                        if (ketQuaTuongTac.KieuTuongTac!="" &&
                            thongTinBenhNhans[i].NgayThang >= DateOnly.FromDateTime(DateTime.Parse(FromDate.ToString())) &&
                            thongTinBenhNhans[i].NgayThang <= DateOnly.FromDateTime(DateTime.Parse(ToDate.ToString())) &&
                            (DateTime.Parse(FromDate.ToString()).Day ==1 ?
                            thongTinBenhNhans[j].NgayThang >= DateOnly.FromDateTime(DateTime.Parse(FromDate.ToString()).AddDays(-15)) :
                            thongTinBenhNhans[j].NgayThang >= DateOnly.FromDateTime(DateTime.Parse(FromDate.ToString()))) &&
                            thongTinBenhNhans[j].NgayThang <= DateOnly.FromDateTime(DateTime.Parse(ToDate.ToString())))
                        {
                            ketQuaTuongTacs.Add(ketQuaTuongTac);
                        }
                    }
                }
            }
            ketQuaTuongTacs = ketQuaTuongTacs.Distinct().ToList();
            return ketQuaTuongTacs;
        }
    }
}
