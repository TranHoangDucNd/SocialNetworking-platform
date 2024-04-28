using System.ComponentModel.DataAnnotations;

namespace WebDating.Entities
{
    public enum Interest
    {
        [Display(Name = "Âm nhạc")]
        Music,
        [Display(Name = "Du lịch")]
        Travel,
        [Display(Name = "Đọc sách")]
        Reading,
        [Display(Name = "Xem phim")]
        WatchingMovies,
        [Display(Name = "Nấu ăn")]
        Cooking,
        [Display(Name = "Thể thao")]
        Sports,
        [Display(Name = "Chơi game")]
        Gaming,
        [Display(Name = "Học ngoại ngữ")]
        LearningLanguages,
        [Display(Name = "Viết blog")]
        Blogging,
        [Display(Name = "Vẽ tranh")]
        Drawing,
        [Display(Name = "Yoga")]
        Yoga,
        [Display(Name = "Thiền")]
        Meditation,
        [Display(Name = "Đi bơi")]
        Swimming,
        [Display(Name = "Làm vườn")]
        Gardening,
        [Display(Name = "Chạy bộ")]
        Running,
        [Display(Name = "Thiết kế đồ họa")]
        GraphicDesign,
        [Display(Name = "Học piano")]
        LearningPiano,
        [Display(Name = "Học guitar")]
        LearningGuitar,
        [Display(Name = "Học vẽ")]
        LearningDrawing,
        [Display(Name = "Lập trình")]
        Programming,
        [Display(Name = "Chụp ảnh")]
        Photography,
        [Display(Name = "Làm thủ công")]
        Crafting,
        [Display(Name = "Trang trí nhà cửa")]
        HomeDecorating,
        [Display(Name = "Thiết kế thời trang")]
        FashionDesign,
        [Display(Name = "Điều khiển drone")]
        DroneFlying,
        [Display(Name = "Bay cao trên trời")]
        Skydiving,
        [Display(Name = "Đi phượt")]
        Backpacking,
        [Display(Name = "Lập kế hoạch")]
        Planning,
        [Display(Name = "Tham gia cộng đồng")]
        CommunityEngagement,
        [Display(Name = "Tập gym")]
        GymWorkout,
        [Display(Name = "Làm từ thiện")]
        CharityWork,
        [Display(Name = "Thi công nghệ")]
        Technology,
        [Display(Name = "Đọc tin tức")]
        NewsReading,
        [Display(Name = "Chơi cờ vua")]
        Chess,
        [Display(Name = "Chơi cờ caro")]
        TicTacToe,
        [Display(Name = "Tham gia đấu trí")]
        BrainTeasers,
        [Display(Name = "Chơi thẻ bài")]
        CardGames,
        [Display(Name = "Học hỏi lịch sử")]
        HistoryLearning,
        [Display(Name = "Tìm hiểu văn hóa")]
        CulturalExploration,
        [Display(Name = "Điều khiển robot")]
        Robotics,
        [Display(Name = "Chăm sóc thú cưng")]
        PetCare,
        [Display(Name = "Tập thể dục")]
        Exercise,
        [Display(Name = "Tham gia hội họa")]
        Painting,
        [Display(Name = "Đi câu cá")]
        Fishing,
        [Display(Name = "Chạy marathon")]
        MarathonRunning,
        [Display(Name = "Học viết")]
        Writing,
        [Display(Name = "Luyện tập sức khỏe")]
        FitnessTraining,
        [Display(Name = "Học múa")]
        Dancing,
        [Display(Name = "Học lập trình")]
        LearningProgramming,
        [Display(Name = "Học nấu ăn")]
        LearningCooking,
        [Display(Name = "Học hát")]
        LearningSinging,
        [Display(Name = "Tham gia tổ chức sự kiện")]
        EventPlanning,
        [Display(Name = "Sưu tập đồ cổ")]
        CollectingAntiques,
        [Display(Name = "Sưu tập tem")]
        StampCollecting,
        [Display(Name = "Chơi đàn")]
        PlayingMusicalInstruments,
        [Display(Name = "Học điện tử")]
        ElectronicsLearning,
        [Display(Name = "Thư giãn")]
        Relaxing,
        [Display(Name = "Xem thể thao")]
        WatchingSports,
        [Display(Name = "Học văn hóa")]
        CulturalLearning,
        [Display(Name = "Chơi thể thao độc lập")]
        IndividualSports,
        [Display(Name = "Chơi thể thao đồng đội")]
        TeamSports,
        [Display(Name = "Chơi thể thao nước")]
        WaterSports,
        [Display(Name = "Thực hành yoga")]
        YogaPractice,
        [Display(Name = "Tập luyện kickboxing")]
        KickboxingTraining,
        [Display(Name = "Tham gia câu lạc bộ")]
        ClubParticipation,
        [Display(Name = "Tham gia hoạt động xã hội")]
        SocialActivities,
        [Display(Name = "Làm từ thiện")]
        Volunteering,
        [Display(Name = "Tham gia nhóm nhảy")]
        DanceGroups,
        [Display(Name = "Tham gia quỹ từ thiện")]
        CharityFunding,
        [Display(Name = "Chơi board games")]
        BoardGaming,
        [Display(Name = "Học sử dụng máy ảnh")]
        CameraSkills,
        [Display(Name = "Luyện tập karate")]
        KarateTraining,
        [Display(Name = "Thực hành múa rối")]
        Puppetry,
        [Display(Name = "Chơi bóng rổ")]
        Basketball,
        [Display(Name = "Chơi bóng chuyền")]
        Volleyball,
        [Display(Name = "Chơi cầu lông")]
        Badminton,
        [Display(Name = "Chơi bóng đá")]
        Football,
        [Display(Name = "Chơi bóng bàn")]
        TableTennis,
        [Display(Name = "Tham gia lớp học thể dục")]
        FitnessClasses,
        [Display(Name = "Chơi quần vợt")]
        Tennis,
        [Display(Name = "Tham gia lớp học yoga")]
        YogaClasses,
        [Display(Name = "Tham gia lớp học đấu vật")]
        WrestlingClasses,
        [Display(Name = "Tham gia lớp học nhảy")]
        DanceClasses,
        [Display(Name = "Tham gia lớp học hát")]
        SingingClasses,
        [Display(Name = "Chơi cờ tướng")]
        ChineseChess,
        [Display(Name = "Chơi cờ cá ngựa")]
        ChineseCheckers,
        [Display(Name = "Chăm sóc chó")]
        DogCare,
        [Display(Name = "Chăm sóc mèo")]
        CatCare,
    }
}
