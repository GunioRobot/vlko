﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;

namespace vlko.core.ValidationAtribute
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class AntiXssAttribute : ActionFilterAttribute, IAuthorizationFilter
	{
		private static readonly AntiXssHtmlTextAttribute AntiXssHtmlText = new AntiXssHtmlTextAttribute();
		private static readonly AntiXssIgnoreAttribute AntiXssIgnore = new AntiXssIgnoreAttribute();
		/// <summary>
		/// Processes the property.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="property">The property.</param>
		private static void ProcessProperty(object value, PropertyDescriptor property)
		{
			if (property.Attributes.Contains(AntiXssHtmlText))
			{
				property.SetValue(value, Sanitizer.GetSafeHtmlFragment((string) property.GetValue(value)));
			}
			else if (property.Attributes.Contains(AntiXssIgnore))
			{
				// do nothing this contains special text
			}
			else
			{
				property.SetValue(value, HttpUtility.HtmlDecode(HttpUtility.HtmlEncode((string)property.GetValue(value))));
			}
		}

		/// <summary>
		/// Called by the MVC framework before the action method executes.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			foreach (KeyValuePair<string, object> actionParameter in filterContext.ActionParameters.ToArray())
			{
				var value = actionParameter.Value;
				if (value is string)
				{
					filterContext.ActionParameters[actionParameter.Key] = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode((string)value));
				}
				else
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
					foreach (PropertyDescriptor property in properties)
					{
						if (property.PropertyType == typeof (string))
						{
							ProcessProperty(value, property);
						}
					}
				}
			}
		}

		/// <summary>
		/// Called when authorization is required.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}
			// disable default validation request
			filterContext.Controller.ValidateRequest = false;
		}
	}
}
