using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace UploadSimples.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase arquivo)
        {
            if (ModelState.IsValid)
            {
                if (arquivo != null)
                {
                    if (arquivo.ContentLength > (1024 * 1024))
                    {
                        ModelState.AddModelError("arquivo", "O tamanho do arquivo não pode ser maior que 1Mb");
                        return View();
                    }

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = Path.GetExtension(arquivo.FileName).Substring(1);


                    if (!supportedTypes.Contains(fileExt.ToLower()))
                    {
                        ModelState.AddModelError("arquivo",
                                                 "Tipo de arquivo invalido, use somente arquivos jpg, jpeg ou png");
                        return View();
                    }

                    //var fileName = Path.GetFileName(arquivo.FileName);//Nome Original do arquivo
                    var fileName = Guid.NewGuid().ToString() + "." + fileExt;//Nome unico

                    var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    
                    //arquivo.SaveAs(path); //somente se não for editar a foto, como no codigo abaixo

                    WebImage imagem = new WebImage(arquivo.InputStream);
                    imagem.Resize(350, 350);
                    //imagem.AddTextWatermark("Cleyton Ferrari");
                    imagem.AddImageWatermark("Content/Uploads/logo.png", 50, 50, "Right", "Bottom", 50, 2);
                    //imagem.Crop(100, 100, 100, 100);
                    imagem.FlipHorizontal();
                    imagem.Save(path);


                    ViewBag.imagem = "Content/Uploads/" + fileName;
                }
            }
            return View();
        }

    }
}
