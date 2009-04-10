using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule
{
    [global::System.Serializable]
    public class ResourceNotFoundException : Exception
    {
        public override string Message
        {
            get
            {
                return "Resource "+ResourceName+" not found.";
            }
        }
        public ResourceNotFoundException(string resourceName) { this.ResourceName = resourceName; }

        public string ResourceName { get; set; }

        public static T Check<T>(T resource, string resourceName) where T : class
        {
            if (resource == null)
                throw new ResourceNotFoundException(resourceName);
            return resource;
        }
    }

    public static class ResourceNotFoundObjectHelper
    {
        public static T Check<T>(this T resource , string resourceName) where T : class
        {
            return ResourceNotFoundException.Check<T>(resource, resourceName);
        }
    }
}
