using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimUserManager.Models;

public class UserViewModel
{
        public UserViewModel _Users { get; set; } = null!;

        [MaxLength(20)]
        [Required]
        [DisplayName("ユーザID")]
        public string UserId { get; set; } = null!;

        [MaxLength(20)]
        [Required]
        [DisplayName("性")]
        public string Lastname { get; set; } = null!;

        [MaxLength(20)]
        [Required]
        [DisplayName("名")]
        public string Firstname { get; set; } = null!;

        [MaxLength(30)]
        [Required]
        [DisplayName("E-Mail")]
        public string Email { get; set; } = null!;

        [MaxLength(30)]
        [DisplayName("役職")]
        public string PositionName { get; set; } = null!;

        [MaxLength(30)]
        [DisplayName("部署")]
        public string Department { get; set; } = null!;

        [MaxLength(30)]
        [DisplayName("課班")]
        public string Section { get; set; } = null!;
}
