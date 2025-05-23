﻿namespace WebApplication1.DTO.product
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
