﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebShop.Models;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.WebApi
{
    [RoutePrefix("api/Photo")]
    public class PhotoController : ApiController
    {
        private IPhotoRepository _photo;
        private IUnitOfWork _unit;

        public PhotoController(IPhotoRepository photo, IUnitOfWork unit)
        {
            _photo = photo;
            _unit = unit;
        }
        [HttpPost]
        [Route("AddPhoto")]
        public async Task<HttpResponseMessage> AddPhoto(int id, FileData file)
        {
            try
            {
                await _photo.Add(id, new MemoryStream(file.Data), file.FileName, file.MimeType);
                _unit.Save();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Accepted,
                string.Format("The photo was added to good - {0} id successfuly", id));
        }

        [HttpPost]
        [Route("RemovePhoto")]
        public HttpResponseMessage RemovePhoto([FromBody]int id)
        {
            try
            {
                _photo.Delete(_photo.FindBy(p => p.PhotoId == id).First());
                _unit.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted,
                    string.Format("The photo by id {0} was deleted successfuly", id));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    string.Format("The photo by id {0} was not deleted", id));
            }
        }
    }
}
