using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PicturesReports.Views
{
    public partial class GaleryPictures : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
            }
        }
        protected static string ReturnEncodedBase64UTF8(object rawImg)
        {
            try
            {
                if (rawImg != null)
                {
                    string img = "data:image/jpeg;base64,{0}"; //change image type if need be
                    Model.Picture picture = (Model.Picture)Convert.ChangeType(rawImg, typeof(Model.Picture));
                    string returnValue = System.Convert.ToBase64String(picture.ImageBinary);
                    return String.Format(img, returnValue);

                }

                return "";
            }
            catch (Exception ex)
            {

                if (rawImg != null)
                {
                    string img = "data:image/jpeg;base64,{0}"; //change image type if need be
                    byte[] toEncodeAsBytes = (byte[])rawImg;
                    string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                    return String.Format(img, returnValue);

                }

                return ""; ;
            }          
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Galery _galery = new Model.Galery()
            {
                Id = Guid.NewGuid(),
                Name = txtName.Value.ToString(),
                Description = txtDescription.Value.ToString(),                
            };

            if (pictureUpload.HasFile)
            {
                Stream fs = pictureUpload.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[]imagemFile = br.ReadBytes((Int32)fs.Length);
                Model.Picture picture = new Model.Picture
                {
                    Id = new Guid(),
                    GaleryId = _galery.Id,
                    ImageBinary = imagemFile
                };
                _galery.Cover = picture;
            }

            if (!String.IsNullOrEmpty(pictureWebCam.Value))
            {
                string imgString = pictureWebCam.Value.Replace("data:image/jpeg;base64,", string.Empty);
                imgString = pictureWebCam.Value.Replace("data:image/png;base64,", string.Empty);
                var imgWebCam = Convert.FromBase64String(imgString);
                Model.Picture picture = new Model.Picture
                {
                    Id = new Guid(),
                    GaleryId = _galery.Id,
                    ImageBinary = imgWebCam
                };
                _galery.Cover = picture;
                pictureWebCam.Value = string.Empty;

            };

            if (Session["listGalery"] == null)
                Session["listGalery"] = new List<Model.Galery>();

            List<Model.Galery> listGaleries = (List<Model.Galery>)Session["listGalery"];
            listGaleries.Add(_galery);

            txtName.Value = String.Empty;
            txtDescription.Value = String.Empty;
            
            Session["listGalery"] = listGaleries;

            gvGalery.DataSource = listGaleries;
            gvGalery.DataBind();
        }

        protected void OpenCam_Click(object sender, EventArgs e)
        {
            upnWebCam.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "function",
                             @"
			                    navigator.mediaDevices.getUserMedia({video:true})
			                    .then(stream => {
				                    console.log(stream)
				                    stream.getTracks().forEach( (track) => {
				                    console.log(track)
					                    track.stop();
				                    });      
			                    })
			                    .catch( (err) =>{
				                    console.log(err);
			                    });   
			                     $('#canvas').hide();
			                     $('#webCam').show();
			                     var webcamConfig = { video: { width: 450, height: 450 } };
			                     navigator.mediaDevices.getUserMedia(webcamConfig)
				                     .then(function(mediaStream) {                                               
						                     var video = document.querySelector('#webCam');
						                     video.srcObject = mediaStream;
						                     video.onloadedmetadata = function(e) {
							                     video.play();
						                     };
				                     })
				                     .catch(function(err) { console.log(err); });
				             ", true);
        }

        protected void btnSalvarFoto_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "function",
                    @"
		                $('#webcam').hide();
		                $('#canvas').show();
		                var webcamConfig = { video: { width: 450, height: 450 } };
		                 navigator.mediaDevices.getUserMedia(webcamConfig)
			                 .then(function(mediaStream) {
					                 var video = document.querySelector('#webcam');                                             
					                  video.srcObject = mediaStream;
					                  var canvas = document.querySelector('#canvas');  
					                  video.onloadedmetadata = function(e) {                                         
						                canvas.height = video.videoHeight;
						                canvas.width = video.videoWidth;
						                var context = canvas.getContext('2d');
						                context.drawImage(video, 0, 0);      
						                var dataURI = canvas.toDataURL('image/jpeg'); 
						                document.querySelector('#pictureWebCam').value  = dataURI; 
						                const tracks = mediaStream.getTracks();
						                tracks.forEach(function(track) {
							                track.stop();
						                });
						                video.srcObject = null;
					                 };
			                 })
	                ", true);
        }

        protected void btnFecharCamera_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "function",
                            @"
			                    var videoEl = document.getElementById('canvas');
			                    stream = videoEl.srcObject;
			                    tracks = stream.getTracks();
			                    tracks.forEach(function(track) {
			                       track.stop();
			                    });
			                    videoEl.srcObject = null;
		                     ", true);

            upnWebCam.Visible = false;
        }

        protected void gvGalery_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Editar":
                        AbrirGaleria(Guid.Parse(e.CommandArgument.ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AbrirGaleria(Guid id)
        {
            List<Model.Galery> listGaleries = (List<Model.Galery>)Session["listGalery"];
            List<Model.Picture> pictures = new List<Model.Picture>();

            Model.Galery galeria = listGaleries.Find(x => x.Id.Equals(id));

            pictures.Add(galeria.Cover);
            if(galeria.Images != null)
            {
                foreach (var picture in galeria.Images)
                {
                    if (picture.ImageBinary == null)
                    {
                        pictures.Add(picture);
                    }
                }
            }
            nameGalery.Text = galeria.Name.ToString();
            rptGaleryPictures.DataSource = pictures;
            rptGaleryPictures.DataBind();
        }
    }
}