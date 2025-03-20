using backend.Middleware;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduConnect.Helpers;

namespace EduConnect.Controllers.Photo
{
    [ApiController]
    [Route("Photo")]
    public class PhotoController(DataContext db, IPhotoService _photoService) : MainController
    {
        [HttpGet("GetAllProfilePictures")]
        public async Task<ActionResult> GetAllProfilePictures()
        {
            var slike = await db.PersonPhoto.ToListAsync();
            if (slike == null)
            {
                return NotFound(new
                {
                    message = "Not photos found on this list",
                    timestamp = DateTime.UtcNow.ToString()

                });
            }

            else
            {
                return Ok(new
                {
                    message = "Request was ok, we successfully got all photos",
                    data = slike,
                    timestamp = DateTime.UtcNow.ToString()


                });
            }

        }
        [CheckPersonLoginSignup]
     
        [HttpGet("GetPhotoForUser/{email}")]
        public async Task<ActionResult> GetProfilePictureForCurrentUser(string email)
        {
            var person = await db.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);

            if (person == null)
            {
                return NotFound(new
                {
                    message = "Cannot find user from token.",
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }

            var photo = await db.PersonPhoto.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            // Ako nema slike, koristi default sliku
            if (photo == null)
            {
                return Ok(new
                {
                    message = "No profile picture found for this user, returning default.",
                    data = new
                    {
                        url = "https://res.cloudinary.com/dsuwjnudy/image/upload/v1735186361/ivbfqfru35jp7m8aeosn.jpg"
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }

            return Ok(new
            {
                message = "Profile picture for this user was successfully found.",
                data = new
                {
                    url = photo.Url
                },
                timestamp = DateTime.UtcNow.ToString("o")
            });
        }




        [CheckPersonLoginSignup]
        [HttpGet("GetCurrentUserProfilePicture")]
        public async Task<ActionResult> GetProfilePictureForCurrentUser()
        {
            var caller = new Caller(this.HttpContext);
            var person = await db.PersonEmail.FirstOrDefaultAsync(x => x.Email == caller.Email);

            if (person == null)
            {
                return NotFound(new
                {
                    message = "Cannot find user from token.",
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }

            var photo = await db.PersonPhoto.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (photo == null)
            {
                return Ok(new
                {
                    message = "No profile picture found for this user, returning default.",
                    data = new
                    {
                        url = "https://res.cloudinary.com/dsuwjnudy/image/upload/v1735186361/ivbfqfru35jp7m8aeosn.jpg"
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }

            return Ok(new
            {
                message = "Profile picture for this user was successfully found.",
                data = new
                {
                    url = photo.Url
                },
                timestamp = DateTime.UtcNow.ToString("o")
            });
        }



        [HttpPost("addPersonProfilePicture")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<PhotoDTO>> AddProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            var caller = new Caller(this.HttpContext);
            var personEmailClaim = caller.Email;
            if (personEmailClaim == null)
            {
                return Unauthorized("User email claim not found.");
            }

            var person = await db.PersonEmail.FirstOrDefaultAsync(x => x.Email == personEmailClaim);
            if (person == null)
            {
                return NotFound("Person not found.");
            }

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var existingUser = await db.PersonPhoto.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            ///If there is not existing user
            if (existingUser == null)
            {

                var photo = new PersonPhoto
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                    PersonId = person.PersonId,
                    ModifiedDate = null,
                };

                db.PersonPhoto.Add(photo);

                if (await db.SaveChangesAsync() > 0)
                {
                    return Ok(new
                    {
                        message = "Photo has been added successfully!",
                        data = new PhotoDTO { publicID = result.PublicId, Url = result.SecureUrl.AbsoluteUri },
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            else
            {
                ///We clear space that on cloudinary! 
                _photoService.DeletePhotoAsync(existingUser.PublicId);
                existingUser.Url = result.SecureUrl.AbsoluteUri;
                existingUser.PublicId = result.PublicId;
                existingUser.ModifiedDate = DateTime.UtcNow.ToString();

                db.PersonPhoto.Update(existingUser);

                if (await db.SaveChangesAsync() > 0)
                {
                    return Ok(new
                    {
                        message = "Updated photo successfully",
                        data = new PhotoDTO { publicID = result.PublicId, Url = result.SecureUrl.AbsoluteUri },
                        timestamp = DateTime.UtcNow
                    });
                }
            }

            return StatusCode(500, "An error occurred while saving the photo.");
        }
    }
}
