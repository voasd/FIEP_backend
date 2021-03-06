﻿using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeletePostRequest : IRequest<ResponseBase>
    {
        public Guid PostId { get; set; }
    }
}
