using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebShop.Repo.Interfaces;
using WebShop.Repo.Repositories;

namespace WebShop.Infostructure.Handlers
{
    public class ImageHandler : HttpTaskAsyncHandler
    {
        private IPhotoRepository _photo;

        public ImageHandler(IPhotoRepository photo)
        {
            _photo = photo;
        }
        

        public override Task ProcessRequestAsync(HttpContext context)
        {
            int id;
            TaskCompletionSource<int> com = new TaskCompletionSource<int>();
            if (int.TryParse(context.Request.Url.Segments[2], out id))
            {
                var photo = _photo.FindBy(p => p.PhotoId == id).FirstOrDefault();
                if (photo != null)
                { 
                    com.SetResult(0);
                    context.Response.ContentType = photo.MimeType;
                    context.Response.BinaryWrite(photo.PhotoByte);
                }
            }
            return com.Task;
        }

        
    }
}