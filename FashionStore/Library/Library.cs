using System.Text.RegularExpressions;
using System.Text;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Library
{
    public static class Library
    {
        // upload image 1 level
        public static string UploadImage(string folderName, IFormFile file)
        {
            var pathSaveImage = "";
            var pathSaveDatabase = "";
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + convertToUnSign3(file.FileName);
                // save image folder
                pathSaveImage = $"wwwroot\\Storage\\{folderName}\\{fileName}";
                // save database 
                pathSaveDatabase = $"Storage/{folderName}/{fileName}";
                using (var stream = new FileStream(pathSaveImage, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return pathSaveDatabase;
        }
        // upload image 2 level
        public static string UploadImage(string folderChild, IFormFile file,string folderParent)
        {
            var pathSaveImage = "";
            var pathSaveDatabase = "";
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                // save image folder
                pathSaveImage = $"wwwroot\\Storage\\{folderParent}\\{folderChild}\\{fileName}";
                // save database 
                pathSaveDatabase = $"Storage/{folderParent}/{folderChild}/{fileName}";
                using (var stream = new FileStream(pathSaveImage, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return pathSaveDatabase;
        }
		// delete image
		public static bool DeleteImage(string pathDelete)
		{
			if (pathDelete != null)
			{
				string path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot", pathDelete);
				File.Delete(path);
				return true;
			}
			else return false;
		}
		// delete image in Folder
		public static bool DeleteImageFolder(string pathDelete)
		{
			if (pathDelete != null)
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(), pathDelete);
				File.Delete(path);
				return true;
			}
			else return false;
		}

		// slug
		public static string convertToUnSign3(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "-").ToLower();
		}
		// mã hóa md5 pass
		public static string MD5(string s)
		{
			using var provider = System.Security.Cryptography.MD5.Create();
			StringBuilder builder = new StringBuilder();

			foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
				builder.Append(b.ToString("x2").ToLower());

			return builder.ToString();
		}
		// send Mail
		public static bool sendMail_UseGmail(string strTo, string strSubject, string strBody)
		{
			MailMessage ms = new MailMessage("pbhuy_20pm@student.agu.edu.vn", strTo, strSubject, strBody);

			ms.BodyEncoding = System.Text.Encoding.UTF8;
			ms.SubjectEncoding = System.Text.Encoding.UTF8;
			ms.IsBodyHtml = true;

#pragma warning disable CS0618 // 'MailMessage.ReplyTo' is obsolete: 'ReplyTo has been deprecated. Use ReplyToList instead, which can accept multiple addresses.'
			ms.ReplyTo = new MailAddress("pbhuy_20pm@student.agu.edu.vn");
#pragma warning restore CS0618 // 'MailMessage.ReplyTo' is obsolete: 'ReplyTo has been deprecated. Use ReplyToList instead, which can accept multiple addresses.'
			ms.Sender = new MailAddress("pbhuy_20pm@student.agu.edu.vn");

			SmtpClient SmtpMail = new SmtpClient("smtp.gmail.com", 587);
			SmtpMail.UseDefaultCredentials = false;
			SmtpMail.Credentials = new NetworkCredential("pbhuy_20pm@student.agu.edu.vn", "Huy0355833767");
			SmtpMail.EnableSsl = true;


#pragma warning disable CS0168 // The variable 'e' is declared but never used
			try
			{
				SmtpMail.Send(ms);
				return true;
			}
			catch (Exception e)
			{
				return false;
				
			}
#pragma warning restore CS0168 // The variable 'e' is declared but never used
		}
		// upload multple image
		public static string uploadMultipleFile(List<IFormFile> files)
		{
			string folderName = Guid.NewGuid().ToString().Substring(5) + "_" + DateTime.Now.ToShortTimeString().Replace(":", "_").Replace(' ','_');

			var pathSave = $"wwwroot\\Storage\\Comment\\{folderName}";

			if (!Directory.Exists(folderName))
				Directory.CreateDirectory(pathSave);
			////////////////////////////
			var pathSaveDB = $"wwwroot/Storage/Comment/" + folderName;

			foreach (var file in files)
			{
				string fileName = file.FileName;
				string saveImage = Path.Combine(pathSave, fileName);
				using (var stream = new FileStream(saveImage, FileMode.Create))
				{
					file.CopyTo(stream);
				}
			}
			return pathSaveDB;
		}
	}
}
