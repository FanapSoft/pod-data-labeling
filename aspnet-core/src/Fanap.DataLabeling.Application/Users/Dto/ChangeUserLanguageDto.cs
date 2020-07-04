using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}