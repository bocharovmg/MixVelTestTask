﻿using TestTask.Domain.Contracts.v1.Dtos;


namespace TestTask.Domain.Contracts.v1.Responses
{
    public class SearchResponse
    {
        // Mandatory
        // Array of routes
        public Route[] Routes { get; set; } = Array.Empty<Route>();

        // Mandatory
        // The cheapest route
        public decimal MinPrice { get; set; }

        // Mandatory
        // Most expensive route
        public decimal MaxPrice { get; set; }

        // Mandatory
        // The fastest route
        public int MinMinutesRoute { get; set; }

        // Mandatory
        // The longest route
        public int MaxMinutesRoute { get; set; }
    }
}
