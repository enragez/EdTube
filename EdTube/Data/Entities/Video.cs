﻿namespace EdTube.Data.Entities;

public class Video
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public byte[] Content { get; set; }
    
    public VideoCategory Category { get; set; }
}