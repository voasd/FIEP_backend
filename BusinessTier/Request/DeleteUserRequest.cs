﻿using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class DeleteUserRequest : IRequest<ResponseBase>
    {
        public Guid UserId { get; set; }
    }
}
