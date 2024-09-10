using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTECH_WebReviewCongNghe.Models
{
    public class PageRS
    {
     
        private List<BAIVIET> dsBV = new List<BAIVIET>();
        private int vitri;
        private int kichThuocTrang;
        private int soLuongTrang;
        private List<DANHMUC> dsDM = new List<DANHMUC>();
        private int index;
        private int pageSize;
        private int pageCount;
        private List<SANPHAM> dsSP= new List<SANPHAM>();
        private int index1;
        private int pageSize1;
        private int pageCount1;

        public List<SANPHAM> DsSP { get => dsSP; set => dsSP = value; }
        public List<DANHMUC> DsDM { get => dsDM; set => dsDM = value; }
        public List<BAIVIET> DsBV { get => dsBV; set => dsBV = value; }

        public int Vitri { get => vitri; set => vitri = value; }
        public int KichThuocTrang { get => kichThuocTrang; set => kichThuocTrang = value; }
        public int SoLuongTrang { get => soLuongTrang; set => soLuongTrang = value; }

        public PageRS(List<BAIVIET> ds, int vtri, int kichThuoc, int sl)
        {
            this.dsBV = ds;
            this.vitri = vtri;
            this.kichThuocTrang = kichThuoc;
            this.soLuongTrang = sl;
        }

        public PageRS(List<SANPHAM> sANPHAMs, int index1, int pageSize1, int pageCount1)
        {
            this.dsSP= sANPHAMs;
            this.vitri = index1;
            this.kichThuocTrang = pageSize1;
            this.soLuongTrang = pageCount1;
        }

        public PageRS(List<DANHMUC> dANHMUCs, int index, int pageSize, int pageCount)
        {
            this.dsDM = dANHMUCs;
            this.index = index;
            this.pageSize = pageSize;
            this.pageCount = pageCount;
        }
    }
}