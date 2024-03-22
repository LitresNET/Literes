﻿using backend.Models;

namespace backend.Abstractions;

public interface IRequestRepository
{
    public Task<Request> AddNewRequestAsync(Request request);
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
    public Request DeleteRequest(Request request);
}