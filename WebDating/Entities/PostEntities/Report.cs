using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities.PostEntities
{
    public enum Report
    {
        [Display(Name = "Nội dung vi phạm quy định về quyền riêng tư")]
        Privacy,
        [Display(Name = "Nội dung xấu, xúc phạm, hay kỳ thị")]
        Offense,
        [Display(Name = "Chứa nội dung bạo lực hoặc đội nhóm xấu")]
        Violence,
        [Display(Name = "Chứa nội dung tự tử hoặc tự gây thương tổn")]
        Suicidal,
        [Display(Name = "Nội dung vi phạm bản quyền hoặc sở hữu trí tuệ")]
        CopyrightInfringement,
        [Display(Name = "Bài đăng chứa thông tin sai lệch hoặc giả mạo")]
        WrongInformation,
        [Display(Name = "Nội dung xuất hiện quá nhiều thông báo hoặc quảng cáo không mong muốn")]
        Advertisement,
    }
}
