﻿using System.ComponentModel.DataAnnotations;
using System.Web;
using ACommunicator.Properties;

namespace ACommunicator.ValidationAttributes
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase).ContentLength <= _maxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(Resources.FileSizeLimit, _maxSize);
        }
    }
}