using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace StoryBoard.Logger
{
    public class BaseLogger
    {
        private ILog _logger;

        protected ILog Logger
        {
            get { return this._logger ?? (this._logger = GetLogger(this.GetType())); }
        }

        private static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type.Name);
        }
    }
}