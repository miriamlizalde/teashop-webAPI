using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TeaShop.Business.Utilities
{
    public class FileValidationHelper
    {
        private readonly string[] _allowedMimeTypes;
        private readonly string[] _allowedExtensions;
        private readonly long     _maxFileSize;

        public FileValidationHelper(string[] allowedMimeTypes,
                                    string[] allowedExtensions,
                                    long maxFileSize = 5 * 1024 * 1024)
        {
            _allowedMimeTypes = allowedMimeTypes;
            _allowedExtensions = allowedExtensions;
            _maxFileSize = maxFileSize;
        }

        public void Validate(IFormFile file)
        {
            if (file.Length <= 0)
                throw new InvalidFileException("El archivo está vacío.");

            if (!_allowedMimeTypes.Contains(file.ContentType))
                throw new InvalidFileException(
                    $"Tipo MIME '{file.ContentType}' no válido. Permitidos: {string.Join(", ", _allowedMimeTypes)}.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidFileException(
                    $"Extensión '{ext}' no válida. Permitidas: {string.Join(", ", _allowedExtensions)}.");

            if (file.Length > _maxFileSize)
                throw new InvalidFileException(
                    $"El archivo supera el máximo de {_maxFileSize / (1024 * 1024)} MB.");
        }
    }

    public class InvalidFileException : Exception
    {
        public InvalidFileException(string message = "") : base(message) { }
    }
}