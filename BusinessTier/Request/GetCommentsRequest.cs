﻿using BusinessTier.Fields;
using BusinessTier.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class GetCommentsRequest : FilterParametersRequest<CommentFields>,IRequest<ResponseBase>
    {
    }
}
