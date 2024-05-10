﻿namespace Litres.Domain.Exceptions;

public class BusinessException(string message) 
    : Exception($"Request couldn't be executed\nInfo: {message}");