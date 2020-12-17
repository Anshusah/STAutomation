using Cicero.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Service.Models.User
{
    public class UserMediaViewModel
    {
        [Key]
            public int Id { get; set; }

            public string UserId { get; set; }

            public int MediaId { get; set; }

            public UserViewModel User { get; set; }

            public MediaViewModel Media { get; set; }
        
    }
}
