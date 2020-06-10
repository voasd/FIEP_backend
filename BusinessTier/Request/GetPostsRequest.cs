﻿using BusinessTier.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetPostsRequest
    {
		//Search param
		public int EventId { get; set; }
		//Paging param
		const int maxPageSize = 10;

		private int _pageSize = 5;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
		public int PageNumber { get; set; } = 1;
		//sort param
		public PostFields Field { get; set; } = 0;
		public Boolean isDesc { get; set; } = true;
	}
}
