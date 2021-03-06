﻿using BusinessTier.DistributedCache;
using BusinessTier.Request;
using BusinessTier.Response;
using BusinessTier.Services;
using DataTier.Models;
using DataTier.UOW;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessTier.Handlers
{
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, ResponseBase>
    {
        private readonly IRedisCacheService _redis;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;
        private readonly bool _cachingEnabled = false;
        private bool CachingEnabled => _cachingEnabled;
        public CreateEventHandler(IUnitOfWork unitOfWork, ICacheStore cacheStore, IRedisCacheService redis)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
            _cachingEnabled = cacheStore != null;
            _redis = redis;
        }
        public async Task<ResponseBase> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var newEvent = new Event()
            {
                EventName = request.EventName,
                ImageUrl = request.EventImageUrl,
                Location = request.Location,
                TimeOccur = request.TimeOccur,
                GroupId = request.GroupId,
            };
            if(DateTime.Compare(DateTime.UtcNow, (DateTime)newEvent.TimeOccur) > 0)
            {
                return new ResponseBase()
                {
                    Response = 0
                };
            }
            _unitOfWork.Repository<Event>().Insert(newEvent);
            var result = _unitOfWork.Commit();
            if (result != 0 && CachingEnabled)
            {
                _redis.CacheEventTable();
            }
            return new ResponseBase()
            {
                Response = 1
            };
        }
    }
}
