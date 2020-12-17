using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class ApiUserToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int UserTokenId { get; set; }

			[Column(TypeName = "varchar(MAX)")]
			public string Token { get; set; }

			[Column(TypeName = "varchar(200)")]
			public string UserId { get; set; }

			[Column(TypeName = "bit")]
			public bool Status { get; set; }

			[Column(TypeName = "datetime2(3)")]
			public DateTime TokenCreatedDate { get; set; }

			[Column(TypeName = "datetime2(3)")]
			public DateTime TokenModifiedDate { get; set; }

			[Column(TypeName = "datetime2(3)")]
			public DateTime TokenExpiryDatetime { get; set; }

    }
}
