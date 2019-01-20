using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Friendster.Controllers.Resources;
using Friendster.Data;
using Friendster.Helpers;
using Friendster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Friendster.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IFriendRepository _repo;
        private IMapper _mapper;
        private IOptions<CloudSettings> _options;
        private Cloudinary _cloudAccount;

        public PhotosController(IFriendRepository repo, IMapper mapper, IOptions<CloudSettings> options)
        {
            _repo = repo;
            _mapper = mapper;
            _options = options;

            Account account = new Account(
                _options.Value.CloudName,
                _options.Value.ApiKey,
                _options.Value.ApiSecret
                );

            _cloudAccount = new Cloudinary(account);
        }

        [HttpGet("{photoId}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var photo = await _repo.GetPhoto(photoId);
            var photoResource = _mapper.Map<Photo, PhotoResource>(photo);
            return Ok(photoResource);
        }

        [HttpPost]
        public async Task<ActionResult> AddPhoto(int userId, [FromForm]AddPhotoResource addPhotoResource)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);

            var file = addPhotoResource.File;
            if (file == null)
            {
                return BadRequest("Photo must be included");
            }

            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudAccount.Upload(uploadParams);
                }
            }

            addPhotoResource.Url = uploadResult.Uri.ToString();
            addPhotoResource.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<AddPhotoResource, Photo>(addPhotoResource);

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _repo.SaveChangesAsync())
            {
                var photoResource = _mapper.Map<Photo, PhotoResource>(photo);

                return CreatedAtRoute(nameof(GetPhoto), new { photoId = photo.Id }, photoResource);
            }

            return BadRequest("Could not add photo to user");
        }

        [HttpPost("{photoId}/setMain")]
        public async Task<ActionResult> SetMainPhoto(int userId, int photoId)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != userId)
            {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(x => x.Id == photoId))
            {
                return Unauthorized();
            }

            var photo = await _repo.GetPhoto(photoId);

            if (photo.IsMain)
            {
                return BadRequest("Photo is already set as main");
            }

            var mainPhoto = await _repo.GetMainPhoto(userId);
            if (mainPhoto == null)
            {
                mainPhoto.IsMain = false;
            }

            photo.IsMain = true;

            if (await _repo.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Something went wrong");
        }
    }
}
