using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuong_Tac_Thuoc.Models;

namespace Tuong_Tac_Thuoc.Utilities
{
    public class TuongTacComparer : IEqualityComparer<ThongTinBenhNhan>
    {
        public bool Equals(ThongTinBenhNhan x, ThongTinBenhNhan y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.MaSo == y.MaSo && 
                   x.HoTen == y.HoTen &&
                   x.NgayThang==y.NgayThang &&
                   x.Thuoc == y.Thuoc;
        }

        public int GetHashCode(ThongTinBenhNhan myObject)
        {
            if (Object.ReferenceEquals(myObject, null)) return 0;

            int hashObjectMaSo = myObject.MaSo.GetHashCode();

            int hashObjectTen = myObject.HoTen == null ? 0 : myObject.HoTen.GetHashCode();

            int hashObjectThuoc = myObject.Thuoc == null ? 0 : myObject.Thuoc.GetHashCode();

            int hashObjectDate = myObject.NgayThang.GetHashCode();

            return hashObjectTen ^ hashObjectMaSo;
        }
    }
}
