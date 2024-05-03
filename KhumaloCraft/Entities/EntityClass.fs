namespace KhumaloCraft.Entities
//  entity class used with DbContext class

open System

type User() =
    member val UserID = 0 with get, set // Primary Key
    member val Email = "" with get, set // Unique
    member val PasswordHash = "" with get, set
    member val CreatedDate = DateTime.Now with get, set

type Product() =
    member val ProductID = 0 with get, set // Primary Key
    member val ProductName = "" with get, set
    member val Description = "" with get, set
    member val Price = 0.0 with get, set
    member val ImageUrl = "" with get, set

type ContactMessage() =
    member val MessageID = 0 with get, set // Primary Key
    member val Name = "" with get, set
    member val Email = "" with get, set
    member val Message = "" with get, set
    member val CreatedDate = DateTime.Now with get, set

