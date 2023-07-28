namespace UserAuthCaller.DTO
{
    public class AuthUpdateDto
    {
        public int userId { get; set; }
        public string? currentPassword { get; set; }
        public string? newPassword { get; set; }


    }
}
