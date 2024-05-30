using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities.ProfileEntities
{
    public class Occupations
    {
        [Key]
        public int Id { get; set; }
        public int DatingProfileId { get; set; }
        public DatingProfile DatingProfile { get; set; }
        public Occupation OccupationName { get; set; }
        public OccupationType OccupationType { get; set; }
    }
    public enum Occupation
    {
        [Display(Name = "Lập trình viên")] laptrinhvien,
        [Display(Name = "Bác sĩ")] bacsi,
        [Display(Name = "Giáo viên")] giaovien,
        [Display(Name = "Kỹ sư")] kysu,
        [Display(Name = "Y tá")] yta,
        [Display(Name = "Kế toán")] ketoan,
        [Display(Name = "Luật sư")] luatsu,
        [Display(Name = "Kiến trúc sư")] kientrucsu,
        [Display(Name = "Dược sĩ")] duocsi,
        [Display(Name = "Thợ điện")] thodien,
        [Display(Name = "Nhân viên kinh doanh")] nhanvienkinhdoanh,
        [Display(Name = "Nhân viên hành chính")] nhanvienhanhchinh,
        [Display(Name = "Nhà báo")] nhabao,
        [Display(Name = "Nhân viên ngân hàng")] nhanviennganhang,
        [Display(Name = "Kỹ thuật viên IT")] kythuatvienit
    }
    public enum OccupationType
    {
        OwnOccupation = 1,
        DesiredOccupation = 2
    }
}
