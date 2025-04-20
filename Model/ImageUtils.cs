namespace PROJET_PIIA.Model {
    internal class ImageLoader {

        static Dictionary<string, Image> _imageCache = new Dictionary<string, Image>();

        public static Image GetImage(string imagePath) {
            if (_imageCache.ContainsKey(imagePath)) {
                return _imageCache[imagePath];
            }

            // Load image and add to cache
            Image img = Image.FromFile(imagePath);
            _imageCache[imagePath] = img;
            return img;
        }


        public static Image GetImageOfMeuble(Meuble meuble) {
            if (meuble == null) throw new ArgumentNullException(nameof(meuble));

            if (!string.IsNullOrEmpty(meuble.ImagePath)) {
                try {
                    Image img = ImageLoader.GetImage(meuble.ImagePath); 
                    return ResizeImageToMeuble(img, meuble);
                } catch (Exception) {
                    return CreateErrorImage(meuble);
                }
            }
            return CreateErrorImage(meuble);
        }

        private static Image ResizeImageToMeuble(Image img, Meuble meuble) {
            Bitmap resizedImage = new Bitmap((int)meuble.Width, (int)meuble.Height);
            using (Graphics g = Graphics.FromImage(resizedImage)) {
                g.DrawImage(img, 0, 0, meuble.Width, meuble.Height);
            }
            return resizedImage;
        }

        private static Image CreateErrorImage(Meuble meuble) {
            Bitmap errorImage = new((int)meuble.Width, (int)meuble.Height);
            using (Graphics g = Graphics.FromImage(errorImage)) {
                g.Clear(Color.Gray);
                using Font font = new("Arial", 8);
                g.DrawString("Image Error", font, Brushes.Red, new PointF(10, meuble.Height / 2 - 10)); // Ajouter du texte rouge
            }
            return errorImage;
        }



        public static void ClearCache() {
            foreach (var img in _imageCache.Values) {
                img.Dispose();
            }
            _imageCache.Clear();
        }

        public static void LoadImagesOfFolder(string folderPath) {
            if (!Directory.Exists(folderPath))
                return;

            string[] files = Directory.GetFiles(folderPath, "*.png"); // or *.jpg, etc.
            foreach (string file in files) {
                try {
                    string key = Path.GetFileNameWithoutExtension(file); // use filename as key
                    Image img = Image.FromFile(file);
                    _imageCache[key] = img;
                } catch (Exception ex) {
                    // Handle invalid image or file access issues
                    Console.WriteLine($"Error loading image {file}: {ex.Message}");
                }
            }
        }

    }
}
