﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IMDbion_MovieHandlerService.Models;

namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public List<MovieActor> Actors { get; set; }
        public string VideoPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
