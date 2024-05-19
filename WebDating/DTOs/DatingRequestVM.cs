using WebDating.Entities.ProfileEntities;

namespace WebDating.DTOs
{
    public class DatingRequestVM
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int CrushId { get; set; }
        public string CrushName { get; set; }
        public string StartDate { get; set; }
        public long DatingTimeSeconds { get; set; }
        public string SenderAvatar { get; set; }
        public string CrushAvatar { get; set; }


        public DatingRequestVM()
        {

        }

        public static DatingRequestVM CreateMap(DatingRequest dating)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dating.ConfirmedDate.Ticks);
            return new DatingRequestVM
            {
                CrushId = dating.CrushId,
                SenderId = dating.SenderId,
                Id = dating.Id,
                StartDate = dating.ConfirmedDate.ToString("yyyy/MM/dd HH:mm:ss"),
                DatingTimeSeconds = (long)ts.TotalSeconds,
            };
        }
    }
}
