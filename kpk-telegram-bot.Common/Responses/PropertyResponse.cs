﻿namespace kpk_telegram_bot.Common.Responses;

public class PropertyResponse
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public PropertyTypeResponse Type { get; set; }
}