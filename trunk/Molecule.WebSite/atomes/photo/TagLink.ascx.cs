﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPhoto.Services;
using WebPhoto.Providers;

namespace Molecule.WebSite.atomes.photo
{
    public partial class TagLink : UserControl
    {

        public ITagInfo Tag { get; set; }

        public bool TextOnly { get; set; }
        
    }
}