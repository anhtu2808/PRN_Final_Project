﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
	public class CreateTicketRequest
	{
		public int OrderId { get; set; }
		public string Content { get; set; }

	}
}
